using OnlineShop.API.Models;
using OnlineShop.API.Repo.Interfaces;

namespace OnlineShop.API.Repo.Repos
{
    public class SellerRepo : ISeller
    {
        public bool CancelBuying(int id)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(Guid buyerGuid, string password)
        {
            throw new NotImplementedException();
        }

        public bool CreateProduct(Guid buyerGuid, Product product, int count)
        {
            throw new NotImplementedException();
        }

        public bool CreateSeller(Seller buyer)
        {
            throw new NotImplementedException();
        }

        public ICollection<SoldProduct> GetProductsToShip(Seller seller)
        {
            throw new NotImplementedException();
        }

        public bool LogIn(string login, string password)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAddress(Buyer buyer)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(string login)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(Guid sellerGuid)
        {
            throw new NotImplementedException();
        }
    }
}
