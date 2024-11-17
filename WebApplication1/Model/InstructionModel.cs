using WebApplication1.Data;

namespace WebApplication1.Model
{
    public class InstructionModel
    {
        public Guid? Id { get; set; }
        public Guid? TourishPlanId { get; set; }
        public Guid? MovingScheduleId { get; set; }
        public Guid? StayingScheduleId { get; set; }
        public InstructionType InstructionType { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
