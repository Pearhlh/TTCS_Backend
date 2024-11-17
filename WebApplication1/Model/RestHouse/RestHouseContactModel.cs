using WebApplication1.Data.RestHouse;

namespace WebApplication1.Model.RestHouse
{
    public class RestHouseContactModel
    {
        public Guid Id { get; set; }
        public required string PlaceBranch { get; set; }
        public RestHouseType RestHouseType { get; set; }
        public string? HotlineNumber { get; set; }
        public string? SupportEmail { get; set; }
        public string? HeadQuarterAddress { get; set; }
        public string? Description { get; set; }
        public float DiscountFloat { get; set; }
        public double DiscountAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
