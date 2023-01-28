using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain;
using OwlStock.Infrastructure.Configuration;

namespace OwlStock.Infrastructure
{
    public class OwlStockDbContext : IdentityDbContext
    {
        public OwlStockDbContext() { }

        public OwlStockDbContext(DbContextOptions<OwlStockDbContext> options)
            : base(options) { }

        public DbSet<Photo>? Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PhotoConfiguration());
            base.OnModelCreating(builder);
        }
    }
}