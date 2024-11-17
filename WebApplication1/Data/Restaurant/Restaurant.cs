using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.RestaurantPlace
{
    [Table("Restaurant")]
    public class Restaurant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string PlaceBranch { get; set; }
        public string? HotlineNumber { get; set; }
        public string? SupportEmail { get; set; }
        public string? HeadQuarterAddress { get; set; }
        public float DiscountFloat { get; set; }
        public double DiscountAmount { get; set; }
        [Required]
        [MaxLength(900)]
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
