using KipCart.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace KipCart.Database
{
    public class KipCartContext : DbContext
    {
        public DbSet<Good> Goods { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseGood> PurchasesGoods { get; set; }

        public KipCartContext(DbContextOptions<KipCartContext> options) : base(options) { }
    }
}
