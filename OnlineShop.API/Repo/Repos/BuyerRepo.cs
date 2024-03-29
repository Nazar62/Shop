using OnlineShop.API.Data;
using OnlineShop.API.Models;
using OnlineShop.API.Models.DTO;
using OnlineShop.API.Repo.Interfaces;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
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

        public bool ProductExists(int id)
        {
            return _context.Products.Any(x => x.Id == id);
        }
        public Product GetProduct(int id)
        {
            return _context.Products.Where(x => x.Id == id).FirstOrDefault();
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

        public bool CreateBuyer(BuyerDTO buyerDTO)
        {
            var buyer = new Buyer()
            {
                Id = 0,
                Name = buyerDTO.Name,
                Password = HashPassword(buyerDTO.Password),
                Email = buyerDTO.Email,
                BuyerGuid = Guid.NewGuid()
            };
            _context.Buyers.Add(buyer);
            return Save();
        }

        public string HashPassword(string pass)
        {
            var sha = SHA256.Create();
            var passBytes = Encoding.UTF8.GetBytes(pass);

            var hashedPass = sha.ComputeHash(passBytes);
            return Convert.ToBase64String(hashedPass);
        }

        public ICollection<SoldProduct> GetShippedProducts(Guid buyerGuid)
        {
            var buyer = _context.Buyers.Where(x => x.BuyerGuid == buyerGuid).FirstOrDefault();
            return _context.SoldProducts.Where(x => x.BuyerID == buyer.Id).ToList();
        }

        public bool LogIn(BuyerDTO buyerDTO)
        {
            var buyer = _context.Buyers.Where(x => x.Name == buyerDTO.Name).FirstOrDefault();
            if(buyer.Password == buyerDTO.Password)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool UpdateAddress(Guid buyerGuid, string address)
        {
            var buyer = _context.Buyers.Where(x => x.BuyerGuid == buyerGuid).FirstOrDefault();
            buyer.Address = address;
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
        public bool UserExists(int id)
        {
            return _context.Buyers.Any(x => x.Id == id);
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

        public bool CreateVerificationToken(string login)
        {
            var buyer = _context.Buyers.Where(x => x.Name == login).FirstOrDefault();
            buyer.VerificationToken = Guid.NewGuid();
            buyer.TokenExpires = DateTime.Now.AddDays(1);
            buyer.TokenVerified = false;
            _context.Buyers.Update(buyer);
            return Save();
        }

        public bool VerifyToken(string login)
        {
            var buyer = _context.Buyers.Where(x => x.Name == login).FirstOrDefault();
            buyer.TokenExpires = DateTime.Now;
            buyer.TokenVerified = true;
            _context.Buyers.Update(buyer);
            return Save();
        }

        public bool CloseVerificationToken(string login)
        {
            var buyer = _context.Buyers.Where(x => x.Name == login).FirstOrDefault();
            buyer.TokenExpires = DateTime.Now;
            buyer.TokenVerified = false;
            _context.Buyers.Update(buyer);
            return Save();
        }

        public Buyer GetBuyer(string login)
        {
            return _context.Buyers.Where(x => x.Name == login).FirstOrDefault();
        }
        public Buyer GetBuyer(Guid buyerGuid)
        {
            return _context.Buyers.Where(x => x.BuyerGuid == buyerGuid).FirstOrDefault();
        }

        public Buyer GetBuyer(int id)
        {
            return _context.Buyers.Where(x => x.Id == id).FirstOrDefault();
        }

        public bool UserExistsEmail(string email)
        {
            return _context.Buyers.Any(x => x.Email == email);
        }

        public bool UpdateBuyer(Buyer buyer)
        {
            _context.Buyers.Update(buyer);
            return Save();
        }

        public Buyer GetBuyerByVerifyToken(Guid buyerToken)
        {
            return _context.Buyers.Where(x => x.VerificationToken == buyerToken).FirstOrDefault();
        }

        public bool DeleteUser(Buyer buyer)
        {
            _context.Buyers.Remove(buyer);
            return Save();
        }

        public SoldProduct GetSoldProduct(int productId)
        {
            return _context.SoldProducts.Where(x => x.Id == productId).FirstOrDefault();
        }

        public bool SoldProductExists(int productId)
        {
            return _context.SoldProducts.Any(x => x.Id == productId);
        }
    }
}
