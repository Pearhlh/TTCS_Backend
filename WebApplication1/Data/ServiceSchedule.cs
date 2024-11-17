using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Receipt;
using WebApplication1.Data.Schedule;

namespace WebApplication1.Data
{
    [Table("ServiceSchedule")]
    public class ServiceSchedule
    {
        public Guid Id { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }
        public ScheduleStatus Status { get; set; }
        public int TotalTicket { get; set; }
        public int RemainTicket { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ICollection<FullScheduleReceipt>? FullScheduleReceiptList { get; set; }
        public MovingSchedule? MovingSchedule { get; set; }
        public StayingSchedule? StayingSchedule { get; set; }
    }
}
