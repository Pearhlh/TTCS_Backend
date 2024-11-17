using WebApplication1.Data.Schedule;
using WebApplication1.Data.Transport;

namespace WebApplication1.Model.Schedule
{
    public class MovingScheduleModel
    {
        public Guid Id { get; set; }
        public Guid? TourishPlanId { get; set; }
        public string? Name { get; set; }
        public string? DriverName { get; set; }
        public string? VehiclePlate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BranchName { get; set; }

        public VehicleType VehicleType { get; set; }
        public Guid TransportId { get; set; }
        public double? SinglePrice { get; set; }

        public string StartingPlace { get; set; }
        public string HeadingPlace { get; set; }
        public ScheduleStatus Status { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public List<InstructionModel>? InstructionList { get; set; }
        public ICollection<ServiceScheduleModel>? ServiceScheduleList { get; set; }

    }
}
