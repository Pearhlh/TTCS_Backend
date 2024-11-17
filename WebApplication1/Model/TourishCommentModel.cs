using WebApplication1.Data;

namespace WebApplication1.Model
{
    public class TourishCommentModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TourishPlanId { get; set; }
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public User? User { get; set; }

    }

    public class TourishCommentDTOModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid TourishPlanId { get; set; }
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public User User { get; set; }

    }
}
