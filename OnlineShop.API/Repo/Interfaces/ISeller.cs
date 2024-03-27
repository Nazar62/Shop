using OnlineShop.API.Models;

namespace OnlineShop.API.Repo.Interfaces
{
    public interface ISeller
    {
        bool CreateSeller(Seller buyer);
        bool LogIn(string login, string password);
        bool UserExists(string login);
        bool UserExists(Guid sellerGuid);
        ICollection<SoldProduct> GetProductsToShip(Seller seller);
        bool CreateProduct(Guid buyerGuid, Product product, int count);
        bool CancelBuying(int id);
        bool UpdateAddress(Buyer buyer);
        bool ChangePassword(Guid buyerGuid, string password);
    }
}
