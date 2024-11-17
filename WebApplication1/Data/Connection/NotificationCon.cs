using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.Connection
{
    [Table("NotificationCon")]
    public class NotificationCon
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public DateTime CreateDate { get; set; }
        // Relationship
        public User? User { get; set; }
    }
}
