using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Schedule;

namespace WebApplication1.Data
{
    public enum InstructionType
    {
        Price = 0, Caution = 1
    }

    [Table("Instruction")]
    public class Instruction
    {
        public Guid Id { get; set; }
        public Guid? TourishPlanId { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }
        public InstructionType InstructionType { get; set; }
        [Column(TypeName = "ntext")]
        public string? Description { get; set; }
        public TourishPlan? TourishPlan { get; set; }
        public MovingSchedule? MovingSchedule { get; set; }
        public StayingSchedule? StayingSchedule { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
