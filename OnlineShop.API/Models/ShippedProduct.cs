using System.ComponentModel.DataAnnotations;

namespace OnlineShop.API.Models
{
    public class ShippedProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public int SellerID { get; set; }
        public Guid SellerKey { get; set; }
        public double AverageRating { get; set; }
    }
}
