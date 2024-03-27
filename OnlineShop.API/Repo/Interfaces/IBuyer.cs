using OnlineShop.API.Models;

namespace OnlineShop.API.Repo.Interfaces
{
    public interface IBuyer
    {
        bool CreateBuyer(Buyer buyer);
        bool LogIn(string login, string password);
        bool UserExists(string login);
        bool UserExists(Guid buyerGuid);
        ICollection<SoldProduct> GetShippedProducts(Buyer buyer);
        bool BuyProduct(Guid buyerGuid, Product product, int count);
        bool CancelBuying(int id);
        bool UpdateAddress(Buyer buyer);
        bool ChangePassword(Guid buyerGuid, string password);
    }
}
