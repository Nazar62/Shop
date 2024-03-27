using OnlineShop.API.Data;
using OnlineShop.API.Models;
using OnlineShop.API.Repo.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace OnlineShop.API.Repo.Repos
{
    public class BuyerRepo : IBuyer
    {
        private readonly MyDbContext _context;
        public BuyerRepo(MyDbContext context)
        {
            _context = context;
        }
        public bool BuyProduct(Guid buyerGuid, Product product, int count)
        {
            var buyer = _context.Buyers.Where(x => x.BuyerGuid == buyerGuid).FirstOrDefault();
            var item = new SoldProduct()
            {
                Id = 0,
                Name = product.Name,
                Price = product.Price,
                Count = count,
                Description = product.Description,
                BuyerID = buyer.Id,
                SellerID = product.SellerID,
                SellerKey = product.SellerKey
            };
            _context.SoldProducts.Add(item);
            return Save();
        }

        public bool CancelBuying(int id)
        {
            _context.SoldProducts.Remove(_context.SoldProducts.Where(x => x.Id == id).FirstOrDefault());
            return Save();
        }

        public bool ChangePassword(Guid buyerGuid, string password)
        {
            Buyer buyer = _context.Buyers.Where(x => x.BuyerGuid == buyerGuid).FirstOrDefault();
            if(buyer != null)
            {
                buyer.Password = password;
                _context.Buyers.Update(buyer);
                return Save();
            } else
            {
                return false;
            }
        }

        public bool CreateBuyer(Buyer buyer)
        {
            _context.Buyers.Add(buyer); //TODO: Add hashing pass
            return Save();
        }

        public ICollection<SoldProduct> GetShippedProducts(Buyer buyer)
        {
            return _context.SoldProducts.Where(x => x.BuyerID == buyer.Id).ToList();
        }

        public bool LogIn(string login, string password)
        {
            var buyer = _context.Buyers.Where(x => x.Name == login).FirstOrDefault();
            if(buyer.Password == password)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool UpdateAddress(Buyer buyer)
        {
            _context.Buyers.Update(buyer);
            return Save();
        }

        public bool UserExists(string login)
        {
            return _context.Buyers.Any(x => x.Name == login);
        }
        public bool UserExists(Guid buyerGuid)
        {
            return _context.Buyers.Any(x => x.BuyerGuid == buyerGuid);
        }
        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
    }
}
