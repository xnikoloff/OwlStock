using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain;

namespace OwlStock.Infrastructure
{
    public class OwlStockDbContext : IdentityDbContext
    {
        public OwlStockDbContext() { }

        public OwlStockDbContext(DbContextOptions<OwlStockDbContext> options)
            : base(options) { }

        public DbSet<Photo>? Photos { get; set; }
    }
}