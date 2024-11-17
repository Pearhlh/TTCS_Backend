namespace WebApplication1.Model
{
    public class TourishRatingModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TourishPlanId { get; set; }
        public int Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
