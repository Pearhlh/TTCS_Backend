using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    [Table("NotificationFcmToken")]
    public class NotificationFcmToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DeviceToken { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        // Relationship
        public User User { get; set; }
    }
}
