using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using WebApplication1.Data;
using WebApplication1.Model;
using WebApplication1.Model.Schedule;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo;
using WebApplication1.Service.InheritanceService;

namespace TourishApi.Service.InheritanceService.Schedule
{
    public class MovingScheduleService
    {
        private readonly TourishOuterScheduleRepository _entityRepository;
        private readonly NotificationService _notificationService;
        private readonly IDatabase _redisDatabase;
        private readonly UserService _userService;

        public MovingScheduleService(
            TourishOuterScheduleRepository entityRepository,
            NotificationService notificationService,
            UserService userService,
            IConnectionMultiplexer connectionMultiplexer
        )
        {
            _entityRepository = entityRepository;
            _notificationService = notificationService;
            _userService = userService;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public async Task<Response> CreateNew(string userId, MovingScheduleModel entityModel)
        {
            try
            {
                var entityExist = _entityRepository.getByNameMovingSchedule(entityModel.BranchName);

                if (entityExist.Data == null)
                {
                    var response = await _entityRepository.AddMovingSchedule(userId, entityModel);

                    var notification = new NotificationModel
                    {
                        UserCreateId = new Guid(userId),
                        UserReceiveId = new Guid(userId),
                        MovingScheduleId = response.returnId,
                        IsGenerate = true,
                        Content = "",
                        ContentCode = "I431",
                        IsRead = false,
                        IsDeleted = false,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };

                    await _notificationService.CreateNewAsync(new Guid(userId), notification);

                    return (response);
                }
                else
                {
                    var response = new Response { resultCd = 1, MessageCode = "C431", };
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 1,
                    MessageCode = "C434",
                    Error = ex.Message
                };
            }
        }

        public async Task<Response> DeleteById(Guid id)
        {
            try
            {
                await _entityRepository.DeleteMovingSchedule(id);
                var response = new Response { resultCd = 0, MessageCode = "I433" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C434",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> GetAll(
            string? search,
            int? type,
            string? endPoint,
            string? startingDate,
            double? priceFrom,
            double? priceTo,
            string? sortBy,
            string? sortDirection,
            string? userId,
            int page = 1,
            int pageSize = 5
        )
        {
            try
            {
                if (String.IsNullOrEmpty(userId))
                {
                    string cacheKey =
                        $"moving_service_list_{search ?? ""}_{type ?? -1}_{endPoint ?? ""}_{startingDate ?? ""}_{priceFrom ?? 0}_{priceTo ?? 0}_{sortBy ?? ""}_{sortDirection ?? ""}_page_{page}_pageSize_{pageSize}";
                    string cachedValue = await _redisDatabase.StringGetAsync(cacheKey);

                    if (!string.IsNullOrEmpty(cachedValue))
                    {
                        var resultCache =
                            JsonConvert.DeserializeObject<WebApplication1.Model.VirtualModel.Response>(
                                cachedValue
                            );
                        if (resultCache != null)
                        {
                            return resultCache;
                        }
                    }

                    var result = _entityRepository.GetAllMovingSchedule(
                        search,
                        type,
                        endPoint,
                        startingDate,
                        priceFrom,
                        priceTo,
                        sortBy,
                        sortDirection,
                        page,
                        pageSize
                    );
                    string resultJson = JsonConvert.SerializeObject(
                        result,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }
                    );

                    // Cache the result in Redis
                    await _redisDatabase.StringSetAsync(
                        cacheKey,
                        resultJson,
                        TimeSpan.FromMinutes(60)
                    ); // Cache for 10 minutes
                    return result;
                }
                else
                {
                    var entityList = _entityRepository.GetAllMovingScheduleWithAuthority(
                        search,
                        type,
                        sortBy,
                        sortDirection,
                        userId,
                        page,
                        pageSize
                    );
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C434",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetById(Guid id)
        {
            try
            {
                var entity = _entityRepository.getByMovingScheduleId(id);
                if (entity.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C430", };
                    return response;
                }
                else
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C434",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> clientGetById(Guid id)
        {
            try
            {
                string cacheKey = $"moving_service_{id}";
                string cachedValue = await _redisDatabase.StringGetAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedValue))
                {
                    var resultCache =
                        JsonConvert.DeserializeObject<WebApplication1.Model.VirtualModel.Response>(
                            cachedValue
                        );
                    if (resultCache != null)
                    {
                        return resultCache;
                    }
                }

                var result = _entityRepository.clientGetByMovingScheduleId(id);
                if (result.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C430", };
                    return response;
                }

                string resultJson = JsonConvert.SerializeObject(
                    result,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                );

                // Cache the result in Redis
                await _redisDatabase.StringSetAsync(cacheKey, resultJson, TimeSpan.FromMinutes(60)); // Cache for 10 minutes

                return result;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C434",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> UpdateEntityById(string userId, MovingScheduleModel entityModel)
        {
            try
            {
                var response = await _entityRepository.UpdateMovingSchedule(userId, entityModel);
                if (response.MessageCode == "I432")
                {
                    var interestList = await _entityRepository.getScheduleInterest(
                        entityModel.Id,
                        ScheduleType.MovingSchedule
                    );

                    foreach (var interest in interestList)
                    {
                        if (interest.InterestStatus == InterestStatus.NotInterested) continue;

                        if (interest.User.Role == UserRole.User)
                        {
                            var isInNeedOfNotify = await _entityRepository.checkArrangeScheduleFromUser(
                                interest.User.Email,
                                entityModel.Id,
                                ScheduleType.MovingSchedule,
                                new List<string>()
                            );
                            if (!isInNeedOfNotify)
                                continue;
                        }

                        if (response.Change != null)
                        {
                            // Create property change notification
                            if (response.Change.propertyChangeList.Count > 0)
                            {
                                await CreateNotification(userId, interest.UserId, entityModel.Id, null, "I432");
                            }

                            // Create schedule change notification
                            if (response.Change.scheduleChangeList.Count > 0 &&
                                interest.User.Role == UserRole.User && await _entityRepository.checkArrangeScheduleFromUser(
                                    interest.User.Email,
                                    entityModel.Id,
                                    ScheduleType.MovingSchedule,
                                    response.Change.scheduleChangeList
                                ))
                            {
                                await CreateNotification(userId, interest.UserId, entityModel.Id, null, "I432-schedule");
                            }

                            if (response.Change.scheduleChangeList.Count > 0 &&
                                (interest.User.Role == UserRole.Admin || interest.User.Role == UserRole.AdminManager))
                            {
                                await CreateNotification(userId, interest.UserId, entityModel.Id, null, "I432-schedule");
                            }

                            if (response.Change.isNewScheduleAdded &&
                               (interest.User.Role == UserRole.Admin || interest.User.Role == UserRole.AdminManager))
                            {
                                await CreateNotification(userId, interest.UserId, entityModel.Id, null, "I432-new-schedule");
                            }
                        }
                    }

                }

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C434",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<int> CreateNotification(string userId, Guid userReceiveId, Guid? movingScheduleId, Guid? stayingScheduleId, string contentCode)
        {
            var notification = new NotificationModel
            {
                UserCreateId = new Guid(userId),
                UserReceiveId = userReceiveId,
                MovingScheduleId = movingScheduleId,
                StayingScheduleId = stayingScheduleId,
                IsGenerate = true,
                Content = "",
                ContentCode = contentCode,
                IsRead = false,
                IsDeleted = false,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _notificationService.CreateNewAsync(userReceiveId, notification);

            return 1;
        }

        public Response getScheduleInterest(Guid scheduleId, Guid userId)
        {
            return _entityRepository.getScheduleInterest(
                scheduleId,
                ScheduleType.MovingSchedule,
                userId
            );
        }

        public Task<Response> setScheduleInterest(
            Guid scheduleId,
            Guid userId,
            InterestStatus interestStatus
        )
        {
            return _entityRepository.setScheduleInterest(
                scheduleId,
                ScheduleType.MovingSchedule,
                userId,
                interestStatus
            );
        }

        public async Task<Response> UpdateInstructionList(
            ScheduleInstructionModel scheduleInstructionModel
        )
        {
            return await _entityRepository.UpdateInstructionList(scheduleInstructionModel);
        }

        public async Task<Response> sendTourPaymentNotifyToAdmin(string email, Guid movingScheduleId, string contentCode)
        {
            var user = (User)_userService.getUserByEmail(email).Data;

            if (user != null)
            {
                var interestList = await _entityRepository.getScheduleInterest(
                        movingScheduleId,
                        ScheduleType.MovingSchedule
                    );

                foreach (var interest in interestList)
                {
                    if (interest.User.Role == UserRole.User)
                    {
                        continue;
                    }

                    await CreateNotification(user.Id.ToString(), interest.UserId, movingScheduleId, null, contentCode);
                }
            }

            return new Response();
        }
    }
}
