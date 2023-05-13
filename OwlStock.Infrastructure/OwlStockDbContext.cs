using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure.Configuration;

namespace OwlStock.Infrastructure
{
    public class OwlStockDbContext : IdentityDbContext
    {
        public OwlStockDbContext() { }

        public OwlStockDbContext(DbContextOptions<OwlStockDbContext> options)
            : base(options) { }

        public DbSet<Photo>? Photos { get; set; }
        public DbSet<PhotoCategory>? PhotosCategories { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<PhotoShoot>? PhotoShoots { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PhotoConfiguration());
            base.OnModelCreating(builder);
        }
    }
}