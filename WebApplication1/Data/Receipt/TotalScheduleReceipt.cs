using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Schedule;

namespace WebApplication1.Data.Receipt
{

    [Table("TotalScheduleReceipt")]
    public class TotalScheduleReceipt
    {
        public Guid TotalReceiptId { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string? Description { get; set; }
        public ReceiptStatus Status { get; set; }
        public MovingSchedule? MovingSchedule { get; set; }
        public StayingSchedule? StayingSchedule { get; set; }
        public ICollection<FullScheduleReceipt> FullReceiptList { get; set; }

        public TotalScheduleReceipt()
        {
            this.FullReceiptList = new List<FullScheduleReceipt>();
        }
    }
}
