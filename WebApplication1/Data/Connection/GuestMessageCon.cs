using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Chat;

namespace WebApplication1.Data.Connection
{
    [Table("GuestMessageCon")]
    public class GuestMessageCon
    {
        [Key]
        public Guid Id { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhoneNumber { get; set; }
        public required string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public int IsChatWithBot { get; set; }
        public bool Connected { get; set; }
        public DateTime CreateDate { get; set; }
        public GuestMessageConHistory GuestMessageConHis { get; set; }
        public ICollection<GuestMessage> GuestMessages { get; set; }
    }
}
