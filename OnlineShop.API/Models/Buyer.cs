using System.ComponentModel.DataAnnotations;

namespace OnlineShop.API.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Guid BuyerGuid { get; set; }
        public Guid VerificationToken { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
