using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Schedule;

namespace WebApplication1.Data
{
    [Table("Notification")]
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid? UserCreateId { get; set; }
        public Guid? UserReceiveId { get; set; }
        public string Content { get; set; }
        public string? ContentCode { get; set; }
        public Boolean IsRead { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Boolean IsGenerate { get; set; }

        public Guid? TourishPlanId { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }
        // Relationship
        public User UserCreator { get; set; }
        public User UserReceiver { get; set; }
        public TourishPlan? TourishPlan { get; set; }
        public MovingSchedule? MovingSchedule { get; set; }
        public StayingSchedule? StayingSchedule { get; set; }
    }
}
