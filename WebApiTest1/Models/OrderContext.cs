using Microsoft.EntityFrameworkCore;

namespace WebApiTest1.Models
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options) { }

        public DbSet<OrderResponse> OrderResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderResponse>()
                .HasOne(or => or.Pickup)
                .WithOne(l => l.OrderResponse)
                .HasForeignKey<Location>(l => l.OrderResponseForeignKey);
        }
    }
}