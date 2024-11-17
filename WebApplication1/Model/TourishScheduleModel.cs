using WebApplication1.Data;

namespace WebApplication1.Model
{
    public class TourishScheduleModel
    {
        public Guid? Id { get; set; }
        public Guid TourishPlanId { get; set; }
        public PlanStatus PlanStatus { get; set; }
        public DateTime StartDate { get; set; }

        public int TotalTicket { get; set; }
        public int RemainTicket { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
