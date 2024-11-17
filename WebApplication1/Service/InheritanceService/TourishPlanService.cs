using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using TourishApi.Service.Interface;
using WebApplication1.Data;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.Interface;
using WebApplication1.Service.InheritanceService;

namespace TourishApi.Service.InheritanceService
{
    public class TourishPlanService : ITourishPlanService
    {
        private readonly ITourishPlanRepository _entityRepository;
        private readonly NotificationService _notificationService;
        private readonly UserService _userService;
        private readonly IDatabase _redisDatabase;

        public TourishPlanService(
            ITourishPlanRepository tourishPlanRepository,
            NotificationService notificationService,
            UserService userService,
            IConnectionMultiplexer connectionMultiplexer
        )
        {
            _userService = userService;
            _entityRepository = tourishPlanRepository;
            _notificationService = notificationService;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public async Task<Response> CreateNew(string userId, TourishPlanInsertModel entityModel)
        {
            try
            {
                var entityExist = _entityRepository.getByName(entityModel.TourName);

                // Lấy ID từ token
                // Tiếp tục xử lý logic của bạn ở đây với userId đã lấy được
                if (entityExist.Data == null)
                {
                    var response = await _entityRepository.Add(entityModel, userId);

                    var notification = new NotificationModel
                    {
                        UserCreateId = new Guid(userId),
                        UserReceiveId = new Guid(userId),
                        TourishPlanId = response.returnId,
                        IsGenerate = true,
                        Content = "",
                        ContentCode = "I411",
                        IsRead = false,
                        IsDeleted = false,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };

                    await _notificationService.CreateNewAsync(new Guid(userId), notification);

                    return response;
                }
                else
                {
                    var response = new Response
                    {
                        resultCd = 1,
                        MessageCode = "C411",
                        Error = "Exist"
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C414",
                    Error = ex.Message,
                    Data = ex
                };
                return response;
            }
        }

        public Response DeleteById(Guid id)
        {
            {
                try
                {
                    _entityRepository.Delete(id);
                    var response = new Response { resultCd = 0, MessageCode = "I413", };
                    return response;
                }
                catch (Exception ex)
                {
                    var response = new Response
                    {
                        resultCd = 1,
                        MessageCode = "C414",
                        Error = ex.Message
                    };
                    return response;
                }
            }
        }

        public async Task<Response> GetAll(
            string? search,
            string? category,
            string? categoryString,
            string? startingPoint,
            string? endPoint,
            string? startingDate,
            double? priceFrom,
            double? priceTo,
            string? sortBy,
            string? sortDirection,
            string? userId,
            int page,
            int pageSize
        )
        {
            try
            {
                if (String.IsNullOrEmpty(userId))
                {
                    string cacheKey =
                        $"tourish_plan_list_{search ?? ""}_{category ?? ""}_{categoryString ?? ""}_{startingPoint ?? ""}_{endPoint ?? ""}_{startingDate ?? ""}_{priceFrom ?? 0}_{priceTo ?? 0}_{sortBy ?? ""}_{sortDirection ?? ""}_page_{page}_pageSize_{pageSize}";
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

                    var result = _entityRepository.GetAll(
                        search,
                        category,
                        categoryString,
                        startingPoint,
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
                    var entityList = _entityRepository.GetAllWithAuthority(
                        search,
                        category,
                        categoryString,
                        startingPoint,
                        endPoint,
                        startingDate,
                        priceFrom,
                        priceTo,
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
                    MessageCode = "C414",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> clientGetById(Guid id)
        {
            try
            {
                string cacheKey = $"tourish_plan_{id}";
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

                var result = _entityRepository.clientGetById(id);
                if (result.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C410", };
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
                    MessageCode = "C414",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetById(Guid id)
        {
            try
            {
                var entity = _entityRepository.getById(id);
                if (entity.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C410", };
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
                    MessageCode = "C414",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> UpdateEntityById(
            string userId,
            TourishPlanUpdateModel entityModel
        )
        {
            try
            {
                var response = await _entityRepository.Update(entityModel, userId);
                if (response.MessageCode == "I412")
                {


                    var interestList = await _entityRepository.getTourInterest(entityModel.Id);

                    foreach (var interest in interestList)
                    {
                        if (interest.InterestStatus == InterestStatus.NotInterested) continue;

                        if (interest.User.Role == UserRole.User)
                        {
                            var isInNeedOfNotify = await _entityRepository.checkArrangeScheduleFromUser(
                                interest.User.Email,
                                entityModel.Id,
                                new List<string>()
                            );
                            if (!isInNeedOfNotify)
                                continue;
                        }

                        if (response.Change != null)
                        {
                            // Create property change notification
                            if (response.Change.serviceChangeList.Count > 0)
                            {
                                var contentCode = "I412-service";
                                if (response.Change.serviceChangeList.Contains("moving"))
                                {
                                    contentCode = contentCode + "-moving";
                                }

                                if (response.Change.serviceChangeList.Contains("staying"))
                                {
                                    contentCode = contentCode + "-staying";
                                }

                                if (response.Change.serviceChangeList.Contains("eating"))
                                {
                                    contentCode = contentCode + "-eating";
                                }

                                await CreateNotification(userId, interest.UserId, entityModel.Id, contentCode);
                            }

                            // Create property change notification
                            if (response.Change.propertyChangeList.Count > 0)
                            {
                                await CreateNotification(userId, interest.UserId, entityModel.Id, "I412");
                            }

                            // Create schedule change notification
                            if (response.Change.scheduleChangeList.Count > 0 &&
                                interest.User.Role == UserRole.User && await _entityRepository.checkArrangeScheduleFromUser(
                                    interest.User.Email,
                                    entityModel.Id,
                                    response.Change.scheduleChangeList
                                ))
                            {
                                await CreateNotification(userId, interest.UserId, entityModel.Id, "I412-schedule");
                            }

                            if (response.Change.scheduleChangeList.Count > 0 &&
                                (interest.User.Role == UserRole.Admin || interest.User.Role == UserRole.AdminManager))
                            {
                                await CreateNotification(userId, interest.UserId, entityModel.Id, "I412-schedule");
                            }

                            if (response.Change.isNewScheduleAdded &&
                               (interest.User.Role == UserRole.Admin || interest.User.Role == UserRole.AdminManager))
                            {
                                await CreateNotification(userId, interest.UserId, entityModel.Id, "I412-new-schedule");
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
                    MessageCode = "C414",
                    Error = ex.Message,
                    Data = ex
                };
                return response;
            }
        }

        // Helper method to create notifications
        public async Task<int> CreateNotification(string userId, Guid userReceiveId, Guid tourishPlanId, string contentCode)
        {
            var notification = new NotificationModel
            {
                UserCreateId = new Guid(userId),
                UserReceiveId = userReceiveId,
                TourishPlanId = tourishPlanId,
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

        public async Task<Response> getDescription(string containerName, string blobName)
        {
            var result = new Response
            {
                resultCd = 0,
                Data = await _entityRepository.getDescription(containerName, blobName)
            };

            return result;
        }

        public async Task<Response> clientGetDescription(string containerName, string blobName)
        {
            string cacheKey = $"tourish_plan_des_{containerName}_{blobName}";
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

            var result = new Response
            {
                resultCd = 0,
                Data = await _entityRepository.getDescription(containerName, blobName)
            };

            string resultJson = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            // Cache the result in Redis
            await _redisDatabase.StringSetAsync(cacheKey, resultJson, TimeSpan.FromMinutes(10));

            return result;
        }

        public Response getTourInterest(Guid tourId, Guid userId)
        {
            return _entityRepository.getTourInterest(tourId, userId);
        }

        public async Task<List<TourishInterest>> getTourInterest(Guid tourId)
        {
            return await _entityRepository.getTourInterest(tourId);
        }

        public Task<Response> setTourInterest(
            Guid tourId,
            Guid userId,
            InterestStatus interestStatus
        )
        {
            return _entityRepository.setTourInterest(tourId, userId, interestStatus);
        }

        public Response getTopTourRating()
        {
            try
            {
                return _entityRepository.getTopTourRating();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C414",
                    Error = ex.Message,
                    Data = ex
                };
                return response;
            }
        }

        public async Task<Response> sendTourPaymentNotifyToAdmin(string email, Guid tourishPlanId, string contentCode)
        {
            var user = (User)_userService.getUserByEmail(email).Data;

            if (user != null)
            {
                var interestList = await _entityRepository.getTourInterest(tourishPlanId);

                foreach (var interest in interestList)
                {
                    if (interest.User.Role == UserRole.User)
                    {
                        continue;
                    }

                    await CreateNotification(user.Id.ToString(), interest.UserId, tourishPlanId, contentCode);
                }
            }

            return new Response();
        }
    }
}
