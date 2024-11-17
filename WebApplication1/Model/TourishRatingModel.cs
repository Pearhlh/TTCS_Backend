namespace WebApplication1.Model
{
    public class ScheduleInstructionModel
    {
        public Guid ScheduleId { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public List<InstructionModel>? InstructionList { get; set; }
    }
}
