using System.ComponentModel.DataAnnotations;

namespace OnlineShop.API.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid Key { get; set; }
    }
}
