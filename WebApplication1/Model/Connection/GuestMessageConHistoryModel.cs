namespace WebApplication1.Model.Connection
{
    public class GuestMessageConHistoryModel
    {
        public Guid Id { get; set; }
        public Guid GuestConId { get; set; }
        public Guid AdminConId { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class GuestMessageConHistoryDTOModel
    {
        public Guid Id { get; set; }
        public GuestMessageConDTOModel GuestMessageCon { get; set; }
        public AdminMessageConDTOModel? AdminMessageCon { get; set; }
        public List<GuestMessageModel>? GuestMessages { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
