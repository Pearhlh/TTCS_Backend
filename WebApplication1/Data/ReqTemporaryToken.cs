using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    public enum TokenPurpose
    {
        SignIn = 0, Reclaim = 1, Payment = 2
    }

    [Table("ReqTemporaryToken")]
    public class ReqTemporaryToken
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Token { get; set; }
        public bool IsActivated { get; set; }
        public TokenPurpose Purpose { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ClosedDate { get; set; }

    }
}
