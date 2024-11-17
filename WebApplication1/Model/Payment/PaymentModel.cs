namespace WebApplication1.Model.Payment
{
    public class PayOsSetting
    {
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string ChecksumKey { get; set; }

        public string ServiceClientId { get; set; }
        public string ServiceApiKey { get; set; }
        public string ServiceChecksumKey { get; set; }
    }

    public class PaymentRequest
    {
        public int orderCode { get; set; }
        public int? amount { get; set; }
        public string? description { get; set; }
        public string? buyerName { get; set; }
        public string? buyerEmail { get; set; }
        public string? buyerPhone { get; set; }
        public string? buyerAddress { get; set; }
        public List<object>? items { get; set; }
        public string? cancelUrl { get; set; }
        public string? returnUrl { get; set; }
        public long? expiredAt { get; set; }
        public string? signature { get; set; }
    }

    public class PaymentResponse
    {
        public string code { get; set; }
        public string desc { get; set; }
        public PaymentData data { get; set; }
        public string signature { get; set; }
    }

    public class PaymentChangeStatusReq
    {
        public string code { get; set; }
        public string id { get; set; }
        public bool cancel { get; set; }
        public string status { get; set; }
        public string orderCode { get; set; }

        public string? productType { get; set; }
    }

    public class PaymentData
    {
        public string bin { get; set; }
        public string accountNumber { get; set; }
        public string accountName { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public int orderCode { get; set; }
        public string currency { get; set; }
        public string paymentLinkId { get; set; }
        public string status { get; set; }
        public string checkoutUrl { get; set; }
        public string qrCode { get; set; }
    }

    public class PaymentGetResponse
    {
        public string code { get; set; }
        public string desc { get; set; }
        public PaymentGetData data { get; set; }
        public string signature { get; set; }
    }

    public class PaymentGetData
    {
        public string id { get; set; }
        public int orderCode { get; set; }
        public int amount { get; set; }
        public int amountPaid { get; set; }
        public int amountRemaining { get; set; }
        public string status { get; set; }
        public string createdAt { get; set; }
        public string? canceledAt { get; set; }
        public string? cancellationReason { get; set; }
        public List<GetTransaction>? transactions { get; set; }
    }

    public class GetTransaction
    {
        public int amount { get; set; }
        public string description { get; set; }
        public string accountNumber { get; set; }
        public string reference { get; set; }
        public string transactionDateTime { get; set; }
        public string? counterAccountBankId { get; set; }
        public string? counterAccountBankName { get; set; }
        public string? counterAccountName { get; set; }
        public string? counterAccountNumber { get; set; }
        public string? virtualAccountName { get; set; }
        public string? virtualAccountNumber { get; set; }
    }

    public class PaymentCancelRequest
    {
        public string? id { get; set; }
        public string? cancellationReason { get; set; }
    }

    public class PaymentWebHookRequest
    {
        public string code { get; set; }
        public string dDesc { get; set; }
        public PaymentWebHookData data { get; set; }
        public string signature { get; set; }
    }

    public class PaymentWebHookData
    {
        public int orderCode { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public string accountNumber { get; set; }
        public string reference { get; set; }
        public string transactionDateTime { get; set; }
        public string currency { get; set; }
        public string paymentLinkId { get; set; }
        public string code { get; set; }
        public string desc { get; set; }
        public string counterAccountBankId { get; set; }
        public string counterAccountBankName { get; set; }
        public string counterAccountName { get; set; }
        public string counterAccountNumber { get; set; }
        public string virtualAccountName { get; set; }
        public string virtualAccountNumber { get; set; }
    }
}

