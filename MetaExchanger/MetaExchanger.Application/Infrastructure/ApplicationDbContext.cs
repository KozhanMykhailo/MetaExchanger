using MetaExchanger.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace MetaExchanger.Application.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CryptoExchange> CryptoExchanges { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.Entity<Order>()
                .HasOne(o => o.CryptoExchange)
                .WithMany(u => u.Bids)
                .HasForeignKey(o => o.CryptoExchangeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.Type)
                .HasDatabaseName("IX_Orders_Type");
        }
    }
}