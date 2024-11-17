using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    public enum InterestStatus
    {
        Creator = 0,
        Modifier = 1,
        Interest = 2,
        User = 3,
        NotInterested = 4,
    }

    [Table("TourishInterest")]
    public class TourishInterest
    {
        public Guid Id { get; set; }
        public Guid TourishPlanId { get; set; }
        public Guid UserId { get; set; }
        public InterestStatus InterestStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public User User { get; set; }
        public TourishPlan? TourishPlan { get; set; }
    }
}
