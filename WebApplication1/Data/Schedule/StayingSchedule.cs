using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Receipt;
using WebApplication1.Data.RestHouse;

namespace WebApplication1.Data.Schedule
{

    [Table("StayingSchedule")]
    public class StayingSchedule
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? TourishPlanId { get; set; }
        public string? Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string? PlaceName { get; set; }
        public string? Address { get; set; }
        public string? SupportNumber { get; set; }

        public RestHouseType RestHouseType { get; set; }
        public Guid RestHouseBranchId { get; set; }
        public double? SinglePrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public TourishPlan TourishPlan { get; set; }
        public ICollection<ScheduleInterest>? ScheduleInterestList { get; set; }
        public ICollection<Notification>? NotificationList { get; set; }
        public ICollection<Instruction>? InstructionList { get; set; }
        public ICollection<ServiceComment> ServiceCommentList { get; set; }

        public ICollection<ServiceSchedule>? ServiceScheduleList { get; set; }
        public TotalScheduleReceipt? TotalReceipt { get; set; }
    }
}
