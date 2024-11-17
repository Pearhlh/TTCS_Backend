using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.Connection
{
    [Table("GuestMessageConHistory")]
    public class GuestMessageConHistory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? GuestConId { get; set; }
        public Guid? AdminConId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CloseDate { get; set; }
        public AdminMessageCon AdminCon { get; set; }
        public GuestMessageCon GuestCon { get; set; }
    }
}
