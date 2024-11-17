using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Authentication;
using WebApplication1.Data.Chat;
using WebApplication1.Data.Connection;

namespace WebApplication1.Data
{
    public enum UserRole
    {
        Cancelled = -1, New = 0, User = 1, AdminTemp = 2, Admin = 3, AdminManager = 4
    }

    [Table("User")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string PasswordSalt { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public required string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? LockoutEnd { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public NotificationFcmToken FcmToken { get; set; }

        public ICollection<RefreshToken> RefreshTokenList { get; set; }
        public ICollection<UserMessage> UserMessageSentList { get; set; }
        public ICollection<UserMessage> UserMessageReceiveList { get; set; }
        public ICollection<Notification> NotificationCreateList { get; set; }
        public ICollection<Notification>? NotificationReceiveList { get; set; }
        public ICollection<TourishInterest> TourishInterests { get; set; }
        public ICollection<NotificationCon> NotificationConList { get; set; }
        public ICollection<UserMessageCon> UserMessageConList { get; set; }

        public ICollection<AdminMessageCon> AdminMessageConList { get; set; }
        public ICollection<TourishComment> TourishCommentList { get; set; }
        public ICollection<ServiceComment> ServiceCommentList { get; set; }
        public ICollection<TourishRating> TourishRatingList { get; set; }
        public ICollection<ScheduleRating> ScheduleRatingList { get; set; }
        public ICollection<ScheduleInterest>? ScheduleInterestList { get; set; }
        public User()
        {
            this.Role = UserRole.New;
        }
    }
}
