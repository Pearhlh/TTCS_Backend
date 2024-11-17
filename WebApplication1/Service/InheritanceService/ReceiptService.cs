using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using TourishApi.Service.InheritanceService.Schedule;
using WebApplication1.Data;
using WebApplication1.Data.Receipt;
using WebApplication1.Data.Schedule;
using WebApplication1.Model;
using WebApplication1.Model.Receipt;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo;
using WebApplication1.Repository.InheritanceRepo.Receipt;
using WebApplication1.Service;
using WebApplication1.Service.InheritanceService;

namespace TourishApi.Service.InheritanceService
{
    public class ReceiptService
    {
        private readonly ReceiptRepository _receiptRepository;
        private readonly ISendMailService _sendMailService;
        private readonly TourishPlanService _tourishPlanService;
        private readonly ILogger<ReceiptService> logger;
        private readonly MovingScheduleService _movingScheduleService;
        private readonly StayingScheduleService _stayingScheduleService;
        private readonly NotificationService _notificationService;
        private readonly UserService _userService;
        private readonly IDatabase _redisDatabase;

        private readonly char[] delimiter = new char[] { ';' };

        public ReceiptService(
            ReceiptRepository receiptRepository,
            ISendMailService sendMailService,
            MovingScheduleService movingScheduleService,
            StayingScheduleService stayingScheduleService,
            ILogger<ReceiptService> _logger,
            IConnectionMultiplexer connectionMultiplexer,
            NotificationService notificationService,
            UserService userService,
            TourishPlanService tourishPlanService,
            TourishPlanRepository tourishPlanRepository
        )
        {
            _receiptRepository = receiptRepository;
            _sendMailService = sendMailService;
            _tourishPlanService = tourishPlanService;
            _movingScheduleService = movingScheduleService;
            _stayingScheduleService = stayingScheduleService;
            _notificationService = notificationService;
            _userService = userService;
            _redisDatabase = connectionMultiplexer.GetDatabase();
            logger = _logger;
        }

        public async Task<Response> CreateNew(FullReceiptInsertModel receiptInsertModel)
        {
            try
            {
                if (receiptInsertModel.TourishPlanId != null)
                {
                    var receiptReturn = await _receiptRepository.AddTourReceipt(receiptInsertModel);

                    var response = new Response { resultCd = 0, MessageCode = "I511", };
                    return response;
                }
                else
                {
                    var receiptReturn = await _receiptRepository.AddServiceReceipt(
                        receiptInsertModel
                    );

                    var response = new Response { resultCd = 0, MessageCode = "I511", };
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C515",
                    Error = ex.Message,
                    Data = ex
                };
                return response;
            }
        }

        public async Task<Response> CreateNewForClient(
            FullReceiptClientInsertModel receiptInsertModel
        )
        {
            try
            {
                if (receiptInsertModel.TourishPlanId != null)
                {
                    var receiptReturn = await _receiptRepository.AddTourReceiptForClient(
                        receiptInsertModel
                    );

                    if (receiptReturn.resultCd == 0)
                        await _tourishPlanService.sendTourPaymentNotifyToAdmin(
                            receiptInsertModel.Email,
                            receiptInsertModel.TourishPlanId.Value,
                            "I511-admin-create"
                        );

                    return receiptReturn;
                }
                else
                {
                    var receiptReturn = await _receiptRepository.AddScheduleReceiptForClient(
                        receiptInsertModel
                    );

                    if (
                        receiptReturn.resultCd == 0
                        && receiptInsertModel.StayingScheduleId.HasValue
                    )
                        await _stayingScheduleService.sendTourPaymentNotifyToAdmin(
                            receiptInsertModel.Email,
                            receiptInsertModel.StayingScheduleId.Value,
                            "I511-admin-create"
                        );
                    if (receiptReturn.resultCd == 0 && receiptInsertModel.MovingScheduleId.HasValue)
                        await _movingScheduleService.sendTourPaymentNotifyToAdmin(
                            receiptInsertModel.Email,
                            receiptInsertModel.MovingScheduleId.Value,
                            "I511-admin-create"
                        );

                    return receiptReturn;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C515",
                    Error = ex.Message,
                    Data = ex
                };
                return response;
            }
        }

        public Response DeleteTourReceiptById(int id)
        {
            try
            {
                _receiptRepository.DeleteTourReceipt(id);
                var response = new Response { resultCd = 0, MessageCode = "I513", };
                return response;
            }
            catch
            {
                var response = new Response { resultCd = 1, MessageCode = "C514", };
                return response;
            }
        }

        public Response DeleteScheduleReceiptById(int id)
        {
            try
            {
                _receiptRepository.DeleteScheduleReceipt(id);
                var response = new Response { resultCd = 0, MessageCode = "I513", };
                return response;
            }
            catch
            {
                var response = new Response { resultCd = 1, MessageCode = "C514", };
                return response;
            }
        }

        public Response GetFullTourReceiptById(int id)
        {
            try
            {
                var receipt = _receiptRepository.getFullTourReceiptById(id);
                if (receipt.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C510", };
                    return response;
                }
                else
                {
                    return receipt;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetFullScheduleReceiptById(int id)
        {
            try
            {
                var receipt = _receiptRepository.getFullScheduleReceiptById(id);
                if (receipt.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C510", };
                    return response;
                }
                else
                {
                    return receipt;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetAllTourReceipt(
            string? tourishPlanId,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5,
            FullReceiptStatus status = FullReceiptStatus.Created
        )
        {
            try
            {
                var receiptList = _receiptRepository.GetAllTourReceipt(
                    tourishPlanId,
                    status,
                    sortBy,
                    sortDirection,
                    page,
                    pageSize
                );
                return receiptList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetAllScheduleReceipt(
            string? movingScheduleId,
            string? stayingScheduleId,
            ScheduleType? scheduleType,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5,
            FullReceiptStatus status = FullReceiptStatus.Created
        )
        {
            try
            {
                var receiptList = _receiptRepository.GetAllScheduleReceipt(
                    movingScheduleId,
                    stayingScheduleId,
                    scheduleType,
                    status,
                    sortBy,
                    sortDirection,
                    page,
                    pageSize
                );
                return receiptList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetAllTourReceiptForUser(
            string? email,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5,
            FullReceiptStatus status = FullReceiptStatus.Created
        )
        {
            try
            {
                var receiptList = _receiptRepository.GetAllTourReceiptForUser(
                    email,
                    status,
                    sortBy,
                    sortDirection,
                    page,
                    pageSize
                );
                return receiptList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetAllScheduleReceiptForUser(
            string? email,
            ScheduleType? scheduleType,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5,
            FullReceiptStatus status = FullReceiptStatus.Created
        )
        {
            try
            {
                var receiptList = _receiptRepository.GetAllScheduleReceiptForUser(
                    email,
                    scheduleType,
                    status,
                    sortBy,
                    sortDirection,
                    page,
                    pageSize
                );
                return receiptList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetTotalTourReceiptById(Guid id)
        {
            try
            {
                var receipt = _receiptRepository.getTotalTourReceiptById(id);
                if (receipt.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C510", };
                    return response;
                }
                else
                {
                    return receipt;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetTotalScheduleReceiptById(Guid id)
        {
            try
            {
                var receipt = _receiptRepository.getTotalScheduleReceiptById(id);
                if (receipt.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C510", };
                    return response;
                }
                else
                {
                    return receipt;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> UpdateReceiptById(
            string emailClaim,
            FullReceiptUpdateModel receiptModel
        )
        {
            try
            {
                var result = new Response();

                var contentCode = "";

                if (receiptModel.TourishPlanId != null)
                {
                    result = await _receiptRepository.UpdateTourReceipt(receiptModel);

                    var existReceipt = (FullReceipt)
                        _receiptRepository.getFullTourReceiptById(receiptModel.FullReceiptId).Data;

                    switch (receiptModel.Status)
                    {
                        case FullReceiptStatus.Created:
                            {
                                contentCode = "I512-user-create";
                                break;
                            }
                        case FullReceiptStatus.AwaitPayment:
                            {
                                if (result.Change != null)
                                {
                                    if (result.Change.propertyChangeList.Contains("Status"))
                                    {
                                        contentCode = "I511-user-await";
                                    }
                                    else contentCode = "I512-user-create";
                                }
                                else contentCode = "I512-user-create";

                                break;
                            }
                        case FullReceiptStatus.Cancelled:
                            {
                                if (result.Change != null)
                                {
                                    if (result.Change.propertyChangeList.Contains("Status"))
                                    {
                                        contentCode = "I511-user-cancel";
                                    }
                                    else contentCode = "I512-user-create";
                                }
                                else contentCode = "I512-user-create";

                                break;
                            }
                        case FullReceiptStatus.Completed:
                            {
                                if (result.Change != null)
                                {
                                    if (result.Change.propertyChangeList.Contains("Status"))
                                    {
                                        contentCode = "I511-user-complete";
                                    }
                                    else contentCode = "I512-user-create";
                                }
                                else contentCode = "I512-user-create";
                                break;
                            }
                        default:
                            // code block
                            break;
                    }

                    if (result.resultCd == 0 && existReceipt != null && result.Change != null)
                    {
                        if (result.Change.propertyChangeList.Count > 0)
                        {
                            await sendTourPaymentNotifyToUser(
                                emailClaim,
                                emailClaim,
                                existReceipt.TotalReceipt.TourishPlanId.Value,
                                contentCode.Replace("user", "admin")
                            );

                            await sendTourPaymentNotifyToUser(
                                emailClaim,
                                existReceipt.Email,
                                existReceipt.TotalReceipt.TourishPlanId.Value,
                                contentCode
                            );
                        }
                    }
                }
                else
                {
                    result = await _receiptRepository.UpdateScheduleReceipt(receiptModel);
                    var existReceipt = (FullScheduleReceipt)
                        _receiptRepository
                            .getFullScheduleReceiptById(receiptModel.FullReceiptId)
                            .Data;

                    switch (receiptModel.Status)
                    {
                        case FullReceiptStatus.Created:
                            {
                                contentCode = "I512-user-create";
                                break;
                            }
                        case FullReceiptStatus.AwaitPayment:
                            {
                                if (result.Change != null)
                                {
                                    if (result.Change.propertyChangeList.Contains("Status"))
                                    {
                                        contentCode = "I511-user-await";
                                    }
                                    else contentCode = "I512-user-create";
                                }
                                else contentCode = "I512-user-create";

                                break;
                            }
                        case FullReceiptStatus.Cancelled:
                            {
                                if (result.Change != null)
                                {
                                    if (result.Change.propertyChangeList.Contains("Status"))
                                    {
                                        contentCode = "I511-user-cancel";
                                    }
                                    else contentCode = "I512-user-create";
                                }
                                else contentCode = "I512-user-create";

                                break;
                            }
                        case FullReceiptStatus.Completed:
                            {
                                if (result.Change != null)
                                {
                                    if (result.Change.propertyChangeList.Contains("Status"))
                                    {
                                        contentCode = "I511-user-complete";
                                    }
                                    else contentCode = "I512-user-create";
                                }
                                else contentCode = "I512-user-create";
                                break;
                            }
                        default:
                            // code block
                            break;
                    }

                    if (result.resultCd == 0 && existReceipt != null && result.Change != null)
                    {
                        if (result.Change.propertyChangeList.Count > 0)
                        {
                            await sendServicePaymentNotifyToUser(
                                    emailClaim,
                                    emailClaim,
                                    existReceipt.TotalReceipt.MovingScheduleId,
                                    existReceipt.TotalReceipt.StayingScheduleId,
                                    contentCode.Replace("user", "admin")
                                );

                            if (existReceipt.TotalReceipt.MovingScheduleId.HasValue)
                                await sendServicePaymentNotifyToUser(
                                    emailClaim,
                                    existReceipt.Email,
                                    existReceipt.TotalReceipt.MovingScheduleId,
                                    null,
                                    contentCode
                                );

                            if (existReceipt.TotalReceipt.StayingScheduleId.HasValue)
                                await sendServicePaymentNotifyToUser(
                                    emailClaim,
                                    existReceipt.Email,
                                    null,
                                    existReceipt.TotalReceipt.StayingScheduleId,
                                    contentCode
                                );
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> UpdateReceiptForUserById(
            Guid fullReceiptId,
            FullReceiptUpdateModel receiptModel
        )
        {
            var result = new Response();
            try
            {
                var contentCode = "";

                switch (receiptModel.Status)
                {
                    case FullReceiptStatus.Created:
                        contentCode = "I512-admin-create";
                        break;
                    case FullReceiptStatus.AwaitPayment:
                        contentCode = "I511-admin-await";
                        break;
                    case FullReceiptStatus.Cancelled:
                        contentCode = "I511-admin-cancel";
                        break;
                    case FullReceiptStatus.Completed:
                        contentCode = "I511-admin-complete";
                        break;
                    default:
                        // code block
                        break;
                }

                if (receiptModel.TourishScheduleId != null)
                {
                    result = await _receiptRepository.UpdateTourReceiptForUser(receiptModel);
                    var existReceipt = (FullReceipt)
                        _receiptRepository.getFullTourReceiptById(receiptModel.FullReceiptId).Data;

                    if (result.resultCd == 0 && result.Change != null)
                    {
                        if (result.Change.propertyChangeList.Count > 0)
                        {
                            await sendTourPaymentNotifyToUser(
                                    existReceipt.Email,
                                    existReceipt.Email,
                                    existReceipt.TotalReceipt.TourishPlanId.Value,
                                    contentCode.Replace("admin", "user")
                                );

                            await _tourishPlanService.sendTourPaymentNotifyToAdmin(
                                existReceipt.Email,
                                existReceipt.TotalReceipt.TourishPlanId.Value,
                                contentCode
                            );
                        }
                    }

                }
                else
                {
                    result = await _receiptRepository.UpdateScheduleReceiptForUser(receiptModel);
                    var existReceipt = (FullScheduleReceipt)
                        _receiptRepository
                            .getFullScheduleReceiptById(receiptModel.FullReceiptId)
                            .Data;

                    if (result.resultCd == 0 && result.Change != null)
                    {
                        if (result.Change.propertyChangeList.Count > 0)
                        {
                            await sendServicePaymentNotifyToUser(
                                                            existReceipt.Email,
                                                            existReceipt.Email,
                                                            existReceipt.TotalReceipt.MovingScheduleId,
                                                            existReceipt.TotalReceipt.StayingScheduleId,
                                                            contentCode.Replace("admin", "user")
                                                        );

                            if (existReceipt.TotalReceipt.StayingScheduleId.HasValue)
                                await _stayingScheduleService.sendTourPaymentNotifyToAdmin(
                                    existReceipt.Email,
                                    existReceipt.TotalReceipt.StayingScheduleId.Value,
                                    contentCode
                                );
                            if (existReceipt.TotalReceipt.MovingScheduleId.HasValue)
                                await _movingScheduleService.sendTourPaymentNotifyToAdmin(
                                    existReceipt.Email,
                                    existReceipt.TotalReceipt.MovingScheduleId.Value,
                                    contentCode
                                );
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message,
                    Data = ex
                };
                return response;
            }
        }

        public Response getUnpaidTourClient()
        {
            try
            {
                return _receiptRepository.getUnpaidTourClient();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getUnpaidMovingScheduleClient()
        {
            try
            {
                return _receiptRepository.getUnpaidMovingScheduleClient();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getUnpaidStayingScheduleClient()
        {
            try
            {
                return _receiptRepository.getUnpaidStayingScheduleClient();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getTopGrossTourInMonth()
        {
            try
            {
                return _receiptRepository.getTopGrossTourInMonth();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getTopTicketTourInMonth()
        {
            try
            {
                return _receiptRepository.getTopTicketTourInMonth();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getTopGrossMovingScheduleInMonth()
        {
            try
            {
                return _receiptRepository.getTopGrossMovingScheduleInMonth();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getTopGrossStayingScheduleInMonth()
        {
            try
            {
                return _receiptRepository.getTopGrossStayingScheduleInMonth();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getTopTicketMovingScheduleInMonth()
        {
            try
            {
                return _receiptRepository.getTopTicketMovingScheduleInMonth();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getTopTicketStayingScheduleInMonth()
        {
            try
            {
                return _receiptRepository.getTopTicketStayingScheduleInMonth();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getGrossStayingScheduleInYear()
        {
            try
            {
                return _receiptRepository.getGrossStayingScheduleInYear();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getGrossMovingScheduleInYear()
        {
            try
            {
                return _receiptRepository.getGrossMovingScheduleInYear();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response getGrossTourishPlanInYear()
        {
            try
            {
                return _receiptRepository.getGrossTourishPlanInYear();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> getTicketOfTourInMonth(Guid tourishPlanId)
        {
            try
            {
                string cacheKey = $"tourish_plan_total_ticket_{tourishPlanId}";
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

                var result = _receiptRepository.getTicketOfTourInMonth(tourishPlanId);
                if (result.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C514", };
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
                await _redisDatabase.StringSetAsync(cacheKey, resultJson, TimeSpan.FromMinutes(60));

                return result;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> thirdPartyPaymentFullServiceReceiptStatusChange(
            string paymentId,
            string orderId,
            string status
        )
        {
            try
            {
                var existReceipt = (FullScheduleReceipt)
                    _receiptRepository.getFullScheduleReceiptById(int.Parse(orderId)).Data;
                if (existReceipt != null)
                {
                    if ((int)existReceipt.Status < 2)
                    {
                        var response =
                            await _receiptRepository.thirdPartyPaymentFullServiceReceiptStatusChange(
                                paymentId,
                                orderId,
                                status
                            );

                        if (status.Equals("PAID"))
                        {
                            await SendServiceReceiptToEmail(orderId);
                            await sendServicePaymentNotifyToUser(
                                existReceipt.Email,
                                existReceipt.Email,
                                existReceipt.ServiceSchedule.MovingScheduleId,
                                existReceipt.ServiceSchedule.StayingScheduleId,
                                "I511-user-complete"
                            );
                        }
                        else if (status.Equals("CANCELLED"))
                        {
                            await sendServicePaymentNotifyToUser(
                                existReceipt.Email,
                                existReceipt.Email,
                                existReceipt.ServiceSchedule.MovingScheduleId,
                                existReceipt.ServiceSchedule.StayingScheduleId,
                                "I511-user-cancel"
                            );

                            if (existReceipt.TotalReceipt.MovingScheduleId.HasValue)
                                await _movingScheduleService.sendTourPaymentNotifyToAdmin(
                                    existReceipt.Email,
                                    existReceipt.TotalReceipt.MovingScheduleId.Value,
                                    "I511-admin-cancel"
                                );

                            if (existReceipt.TotalReceipt.StayingScheduleId.HasValue)
                                await _stayingScheduleService.sendTourPaymentNotifyToAdmin(
                                    existReceipt.Email,
                                    existReceipt.TotalReceipt.StayingScheduleId.Value,
                                    "I511-admin-cancel"
                                );
                        }

                        return response;
                    }
                }
                return new Response();
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> thirdPartyPaymentFullReceiptStatusChange(
            string paymentId,
            string orderId,
            string status
        )
        {
            try
            {
                var existReceipt = (FullReceipt)
                    _receiptRepository.getFullTourReceiptById(int.Parse(orderId)).Data;
                if (existReceipt != null)
                {
                    if ((int)existReceipt.Status < 2)
                    {
                        var response =
                            await _receiptRepository.thirdPartyPaymentFullReceiptStatusChange(
                                paymentId,
                                orderId,
                                status
                            );

                        if (status.Equals("PAID"))
                        {
                            await SendTourReceiptToEmail(orderId);

                            await sendTourPaymentNotifyToUser(
                                existReceipt.Email,
                                existReceipt.Email,
                                existReceipt.TourishSchedule.TourishPlanId,
                                "I511-user-complete"
                            );

                            await _tourishPlanService.sendTourPaymentNotifyToAdmin(
                                existReceipt.Email,
                                existReceipt.TotalReceipt.TourishPlanId.Value,
                                "I511-admin-complete"
                            );
                        }
                        else if (status.Equals("CANCELLED"))
                        {
                            await sendTourPaymentNotifyToUser(
                                existReceipt.Email,
                                existReceipt.Email,
                                existReceipt.TourishSchedule.TourishPlanId,
                                "I511-user-cancel"
                            );

                            await _tourishPlanService.sendTourPaymentNotifyToAdmin(
                                existReceipt.Email,
                                existReceipt.TotalReceipt.TourishPlanId.Value,
                                "I511-admin-cancel"
                            );
                        }

                        return response;
                    }
                }
                return new Response();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C514",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> sendTourPaymentNotifyToUser(
            string emailSend,
            string emailReceive,
            Guid tourishPlanId,
            string contentCode
        )
        {
            var userSend = (User)_userService.getUserByEmail(emailSend).Data;
            var userReceive = (User)_userService.getUserByEmail(emailReceive).Data;

            if (userSend != null)
            {
                var notification = new NotificationModel
                {
                    UserCreateId = userSend.Id,
                    UserReceiveId = userReceive.Id,
                    TourishPlanId = tourishPlanId,
                    IsGenerate = true,
                    Content = "",
                    ContentCode = contentCode,
                    IsRead = false,
                    IsDeleted = false,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                // _notificationService.CreateNew(notification);

                return await _notificationService.CreateNewAsync(userReceive.Id, notification);
            }

            return new Response();
        }

        public async Task<Response> sendServicePaymentNotifyToUser(
            string emailSend,
            string emailReceive,
            Guid? movingServiceId,
            Guid? stayingServiceId,
            string contentCode
        )
        {
            var userSend = (User)_userService.getUserByEmail(emailSend).Data;
            var userReceive = (User)_userService.getUserByEmail(emailReceive).Data;

            if (userSend != null)
            {
                var notification = new NotificationModel
                {
                    UserCreateId = userSend.Id,
                    UserReceiveId = userReceive.Id,
                    MovingScheduleId = movingServiceId,
                    StayingScheduleId = stayingServiceId,
                    IsGenerate = true,
                    Content = "",
                    ContentCode = contentCode,
                    IsRead = false,
                    IsDeleted = false,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                return await _notificationService.CreateNewAsync(userReceive.Id, notification);
            }

            return new Response();
        }

        public async Task<Response> SendTourReceiptToEmail(string orderId)
        {
            try
            {
                var existReceipt = (FullReceipt)
                    _receiptRepository.getFullTourReceiptById(int.Parse(orderId)).Data;

                if (existReceipt != null)
                {
                    var totalPrice =
                        (
                            existReceipt.OriginalPrice * existReceipt.TotalTicket
                            + existReceipt.OriginalPrice * existReceipt.TotalChildTicket
                        ) * (1 - existReceipt.DiscountFloat)
                        - existReceipt.DiscountAmount;

                    var tourishPlan = (TourishPlan)
                        _tourishPlanService
                            .GetById(existReceipt.TourishSchedule.TourishPlanId)
                            .Data;

                    var mailContent = new MailContent
                    {
                        To = existReceipt.Email,
                        Subject = "Roxanne: Thanh toán thành công: " + tourishPlan.TourName,
                        Body =
                            "<html>"
                            + "<head>"
                            + "<style>"
                            + "body { font-family: Arial, sans-serif; background-color: #f4f4f4; }"
                            + ".container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.1); }"
                            + ".message { margin-bottom: 20px; }"
                            + ".message p { margin: 0; font-size: 16px; margin-bottom: 10px; }"
                            + ".btn { display: inline-block; background-color: #007bff; color: #fff !important; text-decoration: none; padding: 10px 20px; border-radius: 5px; }"
                            + "table { width: 100%; border-collapse: collapse; margin-top: 20px; }"
                            + "table, th, td { border: 1px solid #ddd; }"
                            + "th, td { padding: 12px; text-align: left; }"
                            + "th { background-color: #f2f2f2; }"
                            + "</style>"
                            + "</head>"
                            + "<body>"
                            + "<div class='container'>"
                            + "<div class='message'>"
                            + "<p style='font-size: 18px; margin-bottom: 10px;'>Xin chào <strong>"
                            + existReceipt.GuestName
                            + "</strong>.</p>"
                            + "<p style='font-size: 18px; margin-bottom: 10px;'>Xin cảm ơn bạn đã thanh toán dịch vụ của chúng tôi. Sau đây là chi tiết thông tin hóa đơn: </p>"
                            + "<table>"
                            + "<tr>"
                            + "<th>Số vé người lớn</th>"
                            + "<td>"
                            + existReceipt.TotalTicket
                            + "</td>"
                            + "</tr>"
                            + "<tr>"
                            + "<th>Số vé trẻ em</th>"
                            + "<td>"
                            + existReceipt.TotalChildTicket
                            + "</td>"
                            + "</tr>"
                            + "<tr>"
                            + "<th>Tổng giá</th>"
                            + "<td>"
                            + totalPrice
                            + "</td>"
                            + "</tr>"
                            + "<tr>"
                            + "<th>Tên tour</th>"
                            + "<td>"
                            + tourishPlan.TourName
                            + "</td>"
                            + "</tr>"
                            + "<tr>"
                            + "<th>Giá đơn vé</th>"
                            + "<td>"
                            + existReceipt.OriginalPrice
                            + "</td>"
                            + "</tr>"
                            + "</table>"
                            + "</div>"
                            + "</div>"
                            + "</body>"
                            + "</html>"
                    };

                    return await _sendMailService.SendMail(mailContent);
                }

                return new Response();
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }

        public async Task<Response> SendServiceReceiptToEmail(string orderId)
        {
            try
            {
                var existReceipt = (FullScheduleReceipt)
                    _receiptRepository.getFullScheduleReceiptById(int.Parse(orderId)).Data;

                if (existReceipt != null)
                {
                    var totalPrice =
                        (
                            existReceipt.OriginalPrice * existReceipt.TotalTicket
                            + existReceipt.OriginalPrice * existReceipt.TotalChildTicket
                        ) * (1 - existReceipt.DiscountFloat)
                        - existReceipt.DiscountAmount;

                    var serviceName = "";
                    if (existReceipt.ServiceSchedule.MovingScheduleId != null)
                    {
                        var schedule = (MovingSchedule)
                            _movingScheduleService
                                .GetById(
                                    existReceipt.ServiceSchedule.MovingScheduleId ?? new Guid()
                                )
                                .Data;

                        if (schedule != null)
                            serviceName = schedule.Name;
                    }
                    else if (existReceipt.ServiceSchedule.StayingScheduleId != null)
                    {
                        var schedule = (StayingSchedule)
                            _stayingScheduleService
                                .GetById(
                                    existReceipt.ServiceSchedule.StayingScheduleId ?? new Guid()
                                )
                                .Data;

                        if (schedule != null)
                            serviceName = schedule.Name;
                    }

                    var mailContent = new MailContent
                    {
                        To = existReceipt.Email,
                        Subject = "Roxanne: Thanh toán thành công: " + serviceName,
                        Body =
                            "<html>"
                            + "<head>"
                            + "<style>"
                            + "body { font-family: Arial, sans-serif; background-color: #f4f4f4; }"
                            + ".container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.1); }"
                            + ".message { margin-bottom: 20px; }"
                            + ".message p { margin: 0; font-size: 16px; margin-bottom: 10px; }"
                            + ".btn { display: inline-block; background-color: #007bff; color: #fff !important; text-decoration: none; padding: 10px 20px; border-radius: 5px; }"
                            + "table { width: 100%; border-collapse: collapse; margin-top: 20px; }"
                            + "table, th, td { border: 1px solid #ddd; }"
                            + "th, td { padding: 12px; text-align: left; }"
                            + "th { background-color: #f2f2f2; }"
                            + "</style>"
                            + "</head>"
                            + "<body>"
                            + "<div class='container'>"
                            + "<div class='message'>"
                            + "<p style='font-size: 18px; margin-bottom: 10px;'>Xin chào <strong>"
                            + existReceipt.GuestName
                            + "</strong>.</p>"
                            + "<p style='font-size: 18px; margin-bottom: 10px;'>Xin cảm ơn bạn đã thanh toán dịch vụ của chúng tôi. Sau đây là chi tiết thông tin hóa đơn: </p>"
                            + "<table>"
                            + "<tr>"
                            + "<th>Số vé người lớn</th>"
                            + "<td>"
                            + existReceipt.TotalTicket
                            + "</td>"
                            + "</tr>"
                            + "<tr>"
                            + "<th>Số vé trẻ em</th>"
                            + "<td>"
                            + existReceipt.TotalChildTicket
                            + "</td>"
                            + "</tr>"
                            + "<tr>"
                            + "<th>Tổng giá</th>"
                            + "<td>"
                            + totalPrice
                            + "</td>"
                            + "</tr>"
                            + "<tr>"
                            + "<th>Tên dịch vụ</th>"
                            + "<td>"
                            + serviceName
                            + "</td>"
                            + "</tr>"
                            + "<tr>"
                            + "<th>Giá đơn vé</th>"
                            + "<td>"
                            + existReceipt.OriginalPrice
                            + "</td>"
                            + "</tr>"
                            + "</table>"
                            + "</div>"
                            + "</div>"
                            + "</body>"
                            + "</html>"
                    };

                    return await _sendMailService.SendMail(mailContent);
                }

                return new Response();
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }
    }
}
