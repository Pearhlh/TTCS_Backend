using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.Chat
{
    [Table("UserMessage")]
    public class UserMessage
    {
        public Guid Id { get; set; }
        public required Guid UserSentId { get; set; }
        public Guid UserReceiveId { get; set; }
        public Guid? GroupId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        // Relationship
        public User UserSent { get; set; }
        public User UserReceive { get; set; }
        public ICollection<SaveFile>? Files { get; set; }

        public UserMessage()
        {
            Content = "";
            IsRead = false;
            IsDeleted = false;
        }
    }
}
