using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;

namespace OwlStock.Infrastructure
{
    public class PhotonicDbContext : IdentityDbContext
    {
        public PhotonicDbContext() { }

        public PhotonicDbContext(DbContextOptions<PhotonicDbContext> options)
            : base(options) { }

        public DbSet<PhotoBase>? PhotosBase { get; set; }
        public DbSet<GalleryPhoto>? GalleryPhotos { get; set; }
        public DbSet<PhotoShootPhoto>? PhotoShootPhotos { get; set; }
        public DbSet<PhotoCategory>? PhotosCategories { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<PhotoShoot>? PhotoShoots { get; set; }
        public DbSet<Gear>? Gear { get; set; }
        public DbSet<PostCode>? PostCodes { get; set; }
        public DbSet<Region>? Regions { get; set; }
        public DbSet<Municipality>? Municipalities { get; set; }
        public DbSet<City>? Cities { get; set; }
        public DbSet<Article>? Articles { get; set; }
        public DbSet<ArticleCategory>? ArticleCategories { get; set; }
        public DbSet<Place>? Places { get; set; }
        public DbSet<Testimony>? Testimonies { get; set; }
        public DbSet<Announcement>? Announcements { get; set; }
        public DbSet<WorkingTime>? WorkingTime { get; set; }
        public DbSet<GiftCard>? GiftCards { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            /*builder.ApplyConfiguration(new PlaceConfiguration());
            builder.ApplyConfiguration(new DynamicContentCateogyConfiguration());
            builder.ApplyConfiguration(new TestimonyConfiguration());*/
            builder.Entity<PhotoBase>().UseTptMappingStrategy();
        }
    }
}