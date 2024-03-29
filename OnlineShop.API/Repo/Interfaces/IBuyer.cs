using OnlineShop.API.Models;
using OnlineShop.API.Models.DTO;

namespace OnlineShop.API.Repo.Interfaces
{
    public interface IBuyer
    {
        bool CreateBuyer(BuyerDTO buyer);
        bool UpdateBuyer(Buyer buyer);
        string HashPassword(string password);
        bool LogIn(BuyerDTO buyerDTO);
        bool UserExists(string login);
        bool UserExists(Guid buyerGuid);
        bool UserExistsEmail(string email);
        ICollection<SoldProduct> GetShippedProducts(Guid buyerGuid);
        bool BuyProduct(Guid buyerGuid, Product product, int count);
        bool CancelBuying(int id);
        bool ChangePassword(Guid verificationToken, string password);
        bool CreateVerificationToken(string login);
        bool CloseVerificationToken(string login);
        bool VerifyToken(string login);
        Buyer GetBuyer(string login);
        Buyer GetBuyer(int id);
        Buyer GetBuyer(Guid buyerGuid);
        Buyer GetBuyerByVerifyToken(Guid buyerToken);
        bool DeleteUser(Buyer buyer);
    }
}
