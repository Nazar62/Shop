using System.ComponentModel.DataAnnotations;

namespace OnlineShop.API.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
