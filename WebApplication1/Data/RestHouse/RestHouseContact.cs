using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.RestHouse
{
    public enum RestHouseType
    {
        HomeStay = 0, Hotel = 1
    }

    [Table("RestHouseContact")]
    public class RestHouseContact
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string PlaceBranch { get; set; }
        public RestHouseType RestHouseType { get; set; }
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
