using WebApplication1.Data.Receipt;

namespace WebApplication1.Model.Receipt
{
    public class FullReceiptInsertModel
    {
        public Guid? TourishPlanId { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }

        public Guid? TourishScheduleId { get; set; }
        public Guid? ServiceScheduleId { get; set; }
        public int FullReceiptId { get; set; }
        public Guid TotalReceiptId { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double OriginalPrice { get; set; }
        public int TotalTicket { get; set; }
        public int TotalChildTicket { get; set; }
        public FullReceiptStatus Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public float DiscountFloat { get; set; }
        public double DiscountAmount { get; set; }
    }

    public class FullReceiptClientInsertModel
    {
        public Guid? TourishPlanId { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }

        public Guid? TourishScheduleId { get; set; }
        public Guid? ServiceScheduleId { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double OriginalPrice { get; set; }
        public int TotalTicket { get; set; }
        public int TotalChildTicket { get; set; }
    }

    public class FullReceiptUpdateModel
    {
        public Guid? TourishPlanId { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }

        public Guid? TourishScheduleId { get; set; }
        public Guid? ServiceScheduleId { get; set; }
        public int FullReceiptId { get; set; }
        public Guid TotalReceiptId { get; set; }
        public string GuestName { get; set; }
        public double OriginalPrice { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int TotalTicket { get; set; }
        public int TotalChildTicket { get; set; }
        public FullReceiptStatus Status { get; set; }
        public string Description { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public float DiscountFloat { get; set; }
        public double DiscountAmount { get; set; }
    }
}
