namespace WebApplication1.Model
{
    public class NotificationFcmTokenModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DeviceToken { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
