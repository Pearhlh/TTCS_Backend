using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.Authentication
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TokenDescription { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiredDate { get; set; }

        // Relation
        public User User { get; set; }

    }
}
