using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
