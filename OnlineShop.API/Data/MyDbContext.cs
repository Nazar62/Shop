using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;

namespace OnlineShop.API.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
                
        }

        public DbSet<Buyer> Buyers { get; set; }  
        public DbSet<FeedBack> FeedBacks { get; set; }  
        public DbSet<Product> Products { get; set; }  
        public DbSet<Seller> Sellers { get; set; }  
        public DbSet<SoldProduct> SoldProducts { get; set; }  
    }
}
