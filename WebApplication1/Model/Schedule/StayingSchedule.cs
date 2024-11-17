using WebApplication1.Data.RestHouse;

namespace WebApplication1.Model.Schedule
{
    public class StayingScheduleModel
    {
        public Guid Id { get; set; }
        public Guid? TourishPlanId { get; set; }
        public string? Name { get; set; }
        public string? PlaceName { get; set; }
        public string? Address { get; set; }
        public string? SupportNumber { get; set; }

        public RestHouseType RestHouseType { get; set; }
        public Guid RestHouseBranchId { get; set; }
        public double? SinglePrice { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public List<InstructionModel>? InstructionList { get; set; }
        public List<ServiceScheduleModel>? ServiceScheduleList { get; set; }
    }
}
