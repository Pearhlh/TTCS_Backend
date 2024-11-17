using WebApplication1.Data;

namespace WebApplication1.Model
{

    public class TourishPlanModel
    {
        public Guid Id { get; set; }
        public string? TourName { get; set; }
        public string StartingPoint { get; set; }
        public string EndPoint { get; set; }
        public string SupportNumber { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class TourishPlanInsertModel
    {
        public Guid Id { get; set; }
        public string? TourName { get; set; }
        public string StartingPoint { get; set; }
        public string EndPoint { get; set; }
        public string SupportNumber { get; set; }
        public string? Description { get; set; }
        public List<TourishScheduleModel>? TourishScheduleList { get; set; }

        public List<TourishCategoryRelation>? TourishCategoryRelations { get; set; }
        public List<InstructionModel>? InstructionList { get; set; }

        public string EatingScheduleString { get; set; }
        public string MovingScheduleString { get; set; }
        public string StayingScheduleString { get; set; }
    }

    public class TourishPlanUpdateModel
    {
        public Guid Id { get; set; }
        public string? TourName { get; set; }
        public string StartingPoint { get; set; }
        public string EndPoint { get; set; }
        public string SupportNumber { get; set; }
        public string? Description { get; set; }

        public List<TourishCategoryRelation>? TourishCategoryRelations { get; set; }
        public List<TourishScheduleModel>? TourishScheduleList { get; set; }
        public List<InstructionModel>? InstructionList { get; set; }

        public string EatingScheduleString { get; set; }
        public string MovingScheduleString { get; set; }
        public string StayingScheduleString { get; set; }


    }
}
