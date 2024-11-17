using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Schedule;

namespace WebApplication1.Data
{

    [Table("ServiceComment")]
    public class ServiceComment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public User User { get; set; }

        public MovingSchedule MovingSchedule { get; set; }
        public StayingSchedule StayingSchedule { get; set; }
    }
}