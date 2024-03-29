using OnlineShop.API.Models.DTO;

namespace OnlineShop.API.Models.Requests
{
    public class ChangeAddresRequest
    {
        public Guid BuyerGuid { get; set; }
        public string newAddress { get; set; }
    }
}
