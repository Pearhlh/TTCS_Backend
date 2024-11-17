using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using TourishApi.Service.InheritanceService;
using WebApplication1.Data.Receipt;
using WebApplication1.Model;
using WebApplication1.Model.Payment;
using WebApplication1.Service.InheritanceService;

namespace TourishApi.Service.Payment
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly PayOsSetting payOsSettings;
        private readonly AppSetting _appSettings;
        private readonly ILogger<PaymentService> logger;
        private readonly ReceiptService _receiptService;
        private readonly UserService _userService;
        private readonly NotificationService _notificationService;

        public PaymentService(
            HttpClient httpClient,
            IOptions<PayOsSetting> _payOsSettings,
            ILogger<PaymentService> _logger,
            IOptions<AppSetting> appSettings,
            ReceiptService receiptService,
            UserService userService,
            NotificationService notificationService
        )
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            payOsSettings = _payOsSettings.Value;

            if (payOsSettings.ClientId.Length <= 0)
            {
                payOsSettings = new PayOsSetting
                {
                    ClientId = Environment.GetEnvironmentVariable("PAY_OS_CLIENT_ID"),
                    ApiKey = Environment.GetEnvironmentVariable("PAY_OS_API_KEY"),
                    ChecksumKey = Environment.GetEnvironmentVariable("PAY_OS_CHECKSUM_KEY"),
                    ServiceClientId = Environment.GetEnvironmentVariable("PAY_OS_SERVICE_CLIENT_ID"),
                    ServiceApiKey = Environment.GetEnvironmentVariable("PAY_OS_SERVICE_API_KEY"),
                    ServiceChecksumKey = Environment.GetEnvironmentVariable("PAY_OS_SERVICE_CHECKSUM_KEY")
                };
            }

            logger = _logger;
            _receiptService = receiptService;
            _appSettings = appSettings.Value;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<PaymentResponse> MakeTourPaymentAsync(PaymentRequest paymentRequest)
        {
            FullReceipt existReceipt = (FullReceipt)
                _receiptService.GetFullTourReceiptById(paymentRequest.orderCode).Data;

            PaymentRequest insertReq = paymentRequest;
            insertReq.buyerEmail = existReceipt.Email;
            insertReq.buyerPhone = existReceipt.PhoneNumber;
            insertReq.amount = (int)(
                (
                    existReceipt.OriginalPrice * existReceipt.TotalTicket
                    + existReceipt.OriginalPrice * existReceipt.TotalChildTicket / 2
                ) * (1 - existReceipt.DiscountFloat)
                - existReceipt.DiscountAmount
            );

            var token = await _userService.GeneratePaymentToken(paymentRequest.buyerEmail, paymentRequest.orderCode.ToString(), "");

            insertReq.returnUrl = _appSettings.BaseUrl + "/api/CallPayment/pay-os/update/tour/" + token.AccessToken;
            insertReq.cancelUrl = _appSettings.BaseUrl + "/api/CallPayment/pay-os/update/tour/" + token.AccessToken;

            insertReq.buyerName = existReceipt.GuestName;
            insertReq.buyerAddress = "";
            insertReq.items = null;
            insertReq.description = "Roxanne tour thanh toán";

            var response = await MakePaymentAsync(
                insertReq,
                payOsSettings.ClientId,
                payOsSettings.ApiKey,
                payOsSettings.ChecksumKey
            );

            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(response));

            if (response.data != null)
                await _receiptService.thirdPartyPaymentFullReceiptStatusChange(
                    response.data.paymentLinkId,
                    response.data.orderCode.ToString(),
                    response.data.status
                );

            return response;
        }

        public async Task<PaymentResponse> CancelTourPaymentAsync(string id, string reason)
        {
            var response = await CancelPaymentAsync(
                id,
                reason,
                payOsSettings.ClientId,
                payOsSettings.ApiKey
            );

            if (response.data != null)
                await _receiptService.thirdPartyPaymentFullReceiptStatusChange(
                    response.data.paymentLinkId,
                    response.data.orderCode.ToString(),
                    response.data.status
                );

            return response;
        }

        public async Task<PaymentGetResponse> GetTourPaymentRequest(string id)
        {
            var response = await GetPaymentRequest(
                id,
                payOsSettings.ClientId,
                payOsSettings.ApiKey
            );

            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(response));

            if (response.data != null)
                await _receiptService.thirdPartyPaymentFullReceiptStatusChange(
                    response.data.id,
                    response.data.orderCode.ToString(),
                    response.data.status
                );
            return response;
        }

        public async Task<String> CheckTourRequest(string orderId)
        {
            FullReceipt existReceipt = (FullReceipt)
                _receiptService.GetFullTourReceiptById(int.Parse(orderId)).Data;

            if (existReceipt != null)
            {
                if (existReceipt.TourishSchedule.RemainTicket < (existReceipt.TotalTicket + existReceipt.TotalChildTicket))
                {
                    await CancelTourPaymentAsync(orderId, "Không đủ vé để thực hiện giao dịch");

                    return "";
                }
                else return existReceipt.PaymentId ?? "";
            }
            return "";

        }

        public async Task<String> CheckServiceRequest(string orderId)
        {
            FullScheduleReceipt existReceipt = (FullScheduleReceipt)
                _receiptService.GetFullScheduleReceiptById(int.Parse(orderId)).Data;

            if (existReceipt != null)
            {
                return existReceipt.PaymentId ?? "";
            }
            return "";

        }

        public async Task<PaymentResponse> CancelServicePaymentAsync(string id, string reason)
        {
            var response = await CancelPaymentAsync(
                id,
                reason,
                payOsSettings.ServiceClientId,
                payOsSettings.ServiceApiKey
            );
            if (response.data != null)
                logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(response));
            if (response.data != null)
                _receiptService.thirdPartyPaymentFullServiceReceiptStatusChange(
                    response.data.paymentLinkId,
                    response.data.orderCode.ToString(),
                    response.data.status
                );
            return response;
        }

        public async Task<PaymentGetResponse> GetServicePaymentRequest(string id)
        {
            var response = await GetPaymentRequest(
                id,
                payOsSettings.ServiceClientId,
                payOsSettings.ServiceApiKey
            );
            if (response.data != null)
                logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(response));
            if (response.data != null)
                await _receiptService.thirdPartyPaymentFullServiceReceiptStatusChange(
                    response.data.id,
                    response.data.orderCode.ToString(),
                    response.data.status
                );
            return response;
        }

        public async Task<PaymentResponse> MakeServicePaymentAsync(PaymentRequest paymentRequest)
        {
            FullScheduleReceipt existReceipt = (FullScheduleReceipt)
                _receiptService.GetFullScheduleReceiptById(paymentRequest.orderCode).Data;

            PaymentRequest insertReq = paymentRequest;
            insertReq.buyerEmail = existReceipt.Email;
            insertReq.buyerPhone = existReceipt.PhoneNumber;
            insertReq.amount = (int)(
                (
                    existReceipt.OriginalPrice * existReceipt.TotalTicket
                    + existReceipt.OriginalPrice * existReceipt.TotalChildTicket / 2
                ) * (1 - existReceipt.DiscountFloat)
                - existReceipt.DiscountAmount
            );

            var token = await _userService.GeneratePaymentToken(paymentRequest.buyerEmail, "", paymentRequest.orderCode.ToString());

            insertReq.returnUrl = _appSettings.BaseUrl + "/api/CallPayment/pay-os/update/service/" + token.AccessToken;
            insertReq.cancelUrl = _appSettings.BaseUrl + "/api/CallPayment/pay-os/update/service/" + token.AccessToken;

            insertReq.buyerName = existReceipt.GuestName;
            insertReq.buyerAddress = "";
            insertReq.items = null;
            insertReq.description = "Roxanne tour thanh toán";

            var response = await MakePaymentAsync(
                insertReq,
                payOsSettings.ServiceClientId,
                payOsSettings.ServiceApiKey,
                payOsSettings.ServiceChecksumKey
            );
            if (response.data != null)
                await _receiptService.thirdPartyPaymentFullServiceReceiptStatusChange(
                    response.data.paymentLinkId,
                    response.data.orderCode.ToString(),
                    response.data.status
                );

            return response;
        }

        public async Task<PaymentResponse> MakePaymentAsync(
            PaymentRequest paymentRequest,
            string clientId,
            string apiKey,
            string checkSumKey
        )
        {
            if (paymentRequest == null)
            {
                throw new ArgumentNullException(nameof(paymentRequest));
            }
            PaymentRequest insertReq = paymentRequest;

            insertReq.expiredAt = DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds();

            if (insertReq.returnUrl == null)
                insertReq.returnUrl = "https://roxanne-tour.pro/";
            if (insertReq.cancelUrl == null)
                insertReq.cancelUrl = "https://roxanne-tour.pro/";

            insertReq.signature = GenerateSignature(
                paymentRequest.amount.ToString(),
                paymentRequest.cancelUrl,
                paymentRequest.description,
                paymentRequest.orderCode,
                paymentRequest.returnUrl,
                checkSumKey
            );

            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(insertReq));

            // Tạo request body từ paymentRequest
            var requestBody = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(insertReq),
                Encoding.UTF8,
                "application/json"
            );

            // Thêm header x-client-idx-api-key
            _httpClient.DefaultRequestHeaders.Add("x-client-id", clientId);
            _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

            // Gọi API POST
            var response = await _httpClient.PostAsync(
                "https://api-merchant.payos.vn/v2/payment-requests",
                requestBody
            );

            // Đảm bảo response thành công
            response.EnsureSuccessStatusCode();

            // Đọc response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize responseContent to PaymentResponse object
            var responseData = System.Text.Json.JsonSerializer.Deserialize<PaymentResponse>(responseContent);

            return responseData;
        }

        public async Task<PaymentResponse> CancelPaymentAsync(
            string id,
            string reason,
            string clientId,
            string apiKey
        )
        {
            var insertReq = new PaymentCancelRequest();
            insertReq.cancellationReason = reason;

            // Tạo request body từ paymentRequest
            var requestBody = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(insertReq),
                Encoding.UTF8,
                "application/json"
            );

            // Thêm header x-client-idx-api-key
            _httpClient.DefaultRequestHeaders.Add("x-client-id", clientId);
            _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

            // Gọi API POST
            var response = await _httpClient.PostAsync(
                $"https://api-merchant.payos.vn/v2/payment-requests/{id}/cancel",
                requestBody
            );

            // Đảm bảo response thành công
            response.EnsureSuccessStatusCode();

            // Đọc response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize responseContent to PaymentResponse object
            var responseData = System.Text.Json.JsonSerializer.Deserialize<PaymentResponse>(responseContent);

            return responseData;
        }

        public async Task<PaymentGetResponse> GetPaymentRequest(
            string id,
            string clientId,
            string apiKey
        )
        {
            // Thêm header x-client-idx-api-key
            _httpClient.DefaultRequestHeaders.Add("x-client-id", clientId);
            _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

            var response = await _httpClient.GetAsync(
                $"https://api-merchant.payos.vn/v2/payment-requests/{id}"
            );

            var responseContent = await response.Content.ReadAsStringAsync();

            logger.LogInformation(responseContent);

            var responseData = System.Text.Json.JsonSerializer.Deserialize<PaymentGetResponse>(responseContent);

            return responseData;
        }

        public bool isTourWebHookReqValid(PaymentWebHookData data, string signature)
        {
            return IsValidData(data, signature, payOsSettings.ChecksumKey);
        }

        public bool isServiceWebHookReqValid(PaymentWebHookData data, string signature)
        {
            return IsValidData(data, signature, payOsSettings.ServiceChecksumKey);
        }

        public string GenerateSignature(
            string amount,
            string cancelUrl,
            string description,
            int orderCode,
            string returnUrl,
            string checkSumKey
        )
        {
            // Construct the data string in the specified format and sort alphabetically
            var dataString =
                $"amount={amount}&cancelUrl={cancelUrl}&description={description}&orderCode={orderCode}&returnUrl={returnUrl}";
            //var sortedDataString = SortStringAlphabetically(dataString);

            // Convert checksum key and data to bytes
            var keyBytes = Encoding.UTF8.GetBytes(checkSumKey);
            var dataBytes = Encoding.UTF8.GetBytes(dataString);

            // Compute the HMAC_SHA256 hash
            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }

        private Dictionary<string, object> SortObjDataByKey(Dictionary<string, object> data)
        {
            return data.OrderBy(kv => kv.Key)
                       .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        private string ConvertObjToQueryStr(Dictionary<string, object> data)
        {
            var sortedData = SortObjDataByKey(data);
            var queryString = string.Join("&", sortedData.Select(kv =>
            {
                var value = kv.Value ?? string.Empty;
                if (value is JArray jArray)
                {
                    value = JsonConvert.SerializeObject(jArray.OrderBy(x => x.ToString()));
                }
                return $"{kv.Key}={value}";
            }));
            return queryString;
        }

        private bool IsValidData(PaymentWebHookData data, string currentSignature, string checksumKey)
        {
            var dataDict = JObject.FromObject(data).ToObject<Dictionary<string, object>>();
            var sortedDataByKey = SortObjDataByKey(dataDict);
            var dataQueryStr = ConvertObjToQueryStr(sortedDataByKey);
            var dataToSignature = ComputeHmacSha256(dataQueryStr, checksumKey);
            return dataToSignature == currentSignature;
        }

        private string ComputeHmacSha256(string data, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
