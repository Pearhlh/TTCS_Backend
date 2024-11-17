namespace WebApplication1.Model.VirtualModel
{
    public class Response
    {
        public int resultCd { get; set; }
        public string? Error { get; set; }
        public string? MessageCode { get; set; }

        public object? Data { get; set; }

        public int? AveragePoint { get; set; }

        public Guid? returnId { get; set; }
        public int? curId { get; set; }
        public string? type { get; set; }

        public int? count { get; set; }
        public Change? Change { get; set; }
    }

    public class Change
    {
        public List<string> propertyChangeList { get; set; }
        public List<string> scheduleChangeList { get; set; }
        public List<string> serviceChangeList { get; set; }
        public Boolean isNewScheduleAdded { get; set; }


        public Change()
        {
            propertyChangeList = new List<string>();
            scheduleChangeList = new List<string>();
            serviceChangeList = new List<string>();
            isNewScheduleAdded = false; // Assuming default value is false
        }
    }
}
