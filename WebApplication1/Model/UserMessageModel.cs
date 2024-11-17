namespace WebApplication1.Model
{
    public class UserMessageModel
    {
        public Guid Id { get; set; }
        public required Guid UserSentId { get; set; }
        public required Guid UserReceiveId { get; set; }
        public string Content { get; set; }
        public ICollection<FileModel>? Files { get; set; }
        public Boolean IsRead { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class MessageReturnModel
    {
        public Guid Id { get; set; }
        public required Guid UserSentId { get; set; }
        public required Guid UserReceiveId { get; set; }

        public required string UserSentName { get; set; }
        public required string UserReceiveName { get; set; }
        public string Content { get; set; }
        public ICollection<FileModel>? Files { get; set; }
        public Boolean IsRead { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }


}
