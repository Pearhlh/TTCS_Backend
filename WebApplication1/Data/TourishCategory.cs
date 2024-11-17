using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    [Table("TourishCategory")]
    public class TourishCategory
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ICollection<TourishCategoryRelation>? TourishCategoryRelations { get; set; }
    }

    [Table("TourishCategoryRelation")]
    public class TourishCategoryRelation
    {
        public Guid Id { get; set; }

        public Guid TourishPlanId { get; set; }
        public Guid TourishCategoryId { get; set; }
        public TourishPlan? TourishPlan { get; set; }
        public TourishCategory? TourishCategory { get; set; }
    }
}
