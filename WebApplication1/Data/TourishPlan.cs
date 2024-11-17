using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Receipt;
using WebApplication1.Data.Schedule;

namespace WebApplication1.Data
{
    public enum PlanStatus
    {
        Waiting = 0, ConfirmInfo = 1, OnGoing = 2, Complete = 3, Cancel = 4
    }

    [Table("TourishPlan")]
    public class TourishPlan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string? TourName { get; set; }
        public string StartingPoint { get; set; }
        public string EndPoint { get; set; }
        public string SupportNumber { get; set; }

        [Column(TypeName = "ntext")]
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public TotalReceipt TotalReceipt { get; set; }
        public ICollection<Notification> NotificationList { get; set; }

        public ICollection<TourishCategoryRelation> TourishCategoryRelations { get; set; }
        public ICollection<EatSchedule> EatSchedules { get; set; }
        public ICollection<MovingSchedule> MovingSchedules { get; set; }
        public ICollection<StayingSchedule> StayingSchedules { get; set; }
        public ICollection<TourishInterest> TourishInterestList { get; set; }
        public ICollection<TourishComment> TourishCommentList { get; set; }
        public ICollection<TourishRating> TourishRatingList { get; set; }

        public ICollection<TourishSchedule> TourishScheduleList { get; set; }

        public ICollection<Instruction> InstructionList { get; set; }
        public TourishPlan()
        {
            EatSchedules = new List<EatSchedule>();
            MovingSchedules = new List<MovingSchedule>();
            StayingSchedules = new List<StayingSchedule>();
        }
    }
}
