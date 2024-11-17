using WebApplication1.Data;

namespace WebApplication1.Model
{
    public class TourishCategoryModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public TourishPlan? TourishPlan { get; set; }
    }
}
