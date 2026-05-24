using Microsoft.EntityFrameworkCore;
using MPSPDotNetTraining.MiniPosSystem.Models.Entities;

namespace MPSPDotNetTraining.MiniPosSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            // FIX: Changed x.Price to x.UnitPrice
            modelBuilder.Entity<SaleItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            // FIX: Changed x.Subtotal to x.SubTotal
            modelBuilder.Entity<SaleItem>()
                .Property(x => x.SubTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);
        }
    }
}