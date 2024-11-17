namespace WebApplication1.Model
{
    public class ScheduleRatingModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ScheduleId { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public int Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
