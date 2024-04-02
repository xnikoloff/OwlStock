using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;

namespace OwlStock.Infrastructure
{
    public class OwlStockDbContext : IdentityDbContext
    {
        public OwlStockDbContext() { }

        public OwlStockDbContext(DbContextOptions<OwlStockDbContext> options)
            : base(options) { }

        public DbSet<PhotoBase>? PhotosBase { get; set; }
        public DbSet<GalleryPhoto>? GalleryPhotos { get; set; }
        public DbSet<PhotoShootPhoto>? PhotoShootPhotos { get; set; }
        public DbSet<PhotoCategory>? PhotosCategories { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<PhotoShoot>? PhotoShoots { get; set; }
        public DbSet<Gear>? Gear { get; set; }
        public DbSet<PostCode>? PostCodes{ get; set; }
        public DbSet<Region>? Regions{ get; set; }
        public DbSet<Municipality>? Municipalities{ get; set; }
        public DbSet<City>? Cities{ get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfiguration(new PhotoConfiguration());
            builder.Entity<PhotoBase>().UseTptMappingStrategy();
            base.OnModelCreating(builder);
        }
    }
}