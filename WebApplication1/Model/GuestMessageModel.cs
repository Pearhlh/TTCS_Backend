namespace WebApplication1.Model
{
    public class GuestMessageModel
    {
        public Guid? Id { get; set; }
        public int State { get; set; }
        public string Content { get; set; }
        public Boolean? IsRead { get; set; }
        public Boolean? IsDeleted { get; set; }
        public Boolean? IsClosed { get; set; }
        public int? Side { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class GuestMessageReturnModel
    {
        public Guid Id { get; set; }
        public required Guid AdminId { get; set; }
        public required string AdminName { get; set; }
        public string Content { get; set; }
        public Boolean IsRead { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }


}
