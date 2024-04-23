using Microsoft.EntityFrameworkCore;
using MiniShoppingCartAPI.Entities;

namespace MiniShoppingCartAPI.DbContexts
{
    public class MiniShoppingCartApiContextSQLExpress : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public MiniShoppingCartApiContextSQLExpress(DbContextOptions<MiniShoppingCartApiContextSQLExpress> options) : base(options) { }

        public MiniShoppingCartApiContextSQLExpress()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(

               new Product("Apple", 10, 100)
               {
                   Pid = Guid.NewGuid()
               },
                new Product("Orange", 20, 50)
                {
                    Pid = Guid.NewGuid()
                },
                 new Product("Banana", 50, 10)
                 {
                     Pid = Guid.NewGuid()
                 },
                  new Product("Watermelon", 5, 200)
                  {
                      Pid = Guid.NewGuid()
                  },
                   new Product("Strawberry", 100, 30)
                   {
                       Pid = Guid.NewGuid()
                   }

               );
        }
    }
}
