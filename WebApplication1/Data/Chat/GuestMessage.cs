using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Connection;

namespace WebApplication1.Data.Chat
{
    [Table("GuestMessage")]
    public class GuestMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public Guid? GuestMessageConId { get; set; }
        public Guid? AdminMessageConId { get; set; }

        public GuestMessageCon? GuestMessageCon { get; set; }
        public AdminMessageCon? AdminMessageCon { get; set; }

        public GuestMessage()
        {
            Content = "";
            IsRead = false;
            IsDeleted = false;
        }
    }
}
