namespace OnlineShop.API.Models.Requests
{
    public class CancelBuyRequest
    {
        public int SoldId { get; set; }
        public int BuyerId { get; set; }
        public Guid BuyerGuid { get; set; }
    }
}
