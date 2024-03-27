namespace OnlineShop.API.Models
{
    public class SoldProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsShipped { get; set; } = false;
        public string PostCode { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public int BuyerID { get; set; }
        public int SellerID { get; set; }
        public Guid SellerKey { get; set; }
    }
}
