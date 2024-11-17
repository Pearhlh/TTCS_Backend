using WebApplication1.Data.Schedule;

namespace WebApplication1.Model
{
    public class ServiceScheduleModel
    {
        public Guid? Id { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }
        public int TotalTicket { get; set; }
        public int RemainTicket { get; set; }
        public ScheduleStatus Status { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
