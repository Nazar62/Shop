namespace OnlineShop.API.Models.Requests
{
    public class BuyProductRequest
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public Guid BuyerGuid { get; set; }
    }
}
