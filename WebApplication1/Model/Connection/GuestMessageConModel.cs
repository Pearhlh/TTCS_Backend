namespace WebApplication1.Model.Connection
{
    public class GuestMessageConModel
    {
        public Guid Id { get; set; }
        public Guid GuestConHisId { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhoneNumber { get; set; }
        public required string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class AdminMessageConDTOModel
    {
        public Guid Id { get; set; }
        public string AdminFullName { get; set; }
        public Guid? AdminId { get; set; }
        public required string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public ICollection<GuestMessageModel>? GuestMessages { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class GuestMessageConDTOModel
    {
        public Guid Id { get; set; }
        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }
        public string? GuestPhoneNumber { get; set; }
        public required string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public ICollection<GuestMessageModel>? GuestMessages { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
