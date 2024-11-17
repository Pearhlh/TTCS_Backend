using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Chat;

namespace WebApplication1.Data.Connection
{
    [Table("AdminMessageCon")]
    public class AdminMessageCon
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? AdminId { get; set; }
        public required string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public DateTime CreateDate { get; set; }
        // Relationship
        public User Admin { get; set; }
        public GuestMessageConHistory GuestMessageConHis { get; set; }
        public ICollection<GuestMessage> GuestMessages { get; set; }
    }
}
