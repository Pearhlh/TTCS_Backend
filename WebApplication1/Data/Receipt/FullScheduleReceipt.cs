using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.Receipt
{

    [Table("FullScheduleReceipt")]
    public class FullScheduleReceipt
    {
        [Key]
        public int FullReceiptId { get; set; }
        public Guid TotalReceiptId { get; set; }
        public Guid? ServiceScheduleId { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double OriginalPrice { get; set; }
        public int TotalTicket { get; set; }
        public int TotalChildTicket { get; set; }
        public FullReceiptStatus Status { get; set; }
        [MaxLength(900)]
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public float DiscountFloat { get; set; }
        public double DiscountAmount { get; set; }
        public string? PaymentId { get; set; }
        // Relationship
        public TotalScheduleReceipt TotalReceipt { get; set; }
        public ServiceSchedule ServiceSchedule { get; set; }
        public FullScheduleReceipt()
        {
            TotalTicket = 1;
        }

    }
}
