using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;

namespace OwlStock.Tests
{
    public class DataSeeder
    {
        protected OwlStockDbContext BuildDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OwlStockDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            OwlStockDbContext context = new(optionsBuilder);
            return context;

        }

        public async Task<OwlStockDbContext> ArrangeDbContext()
        {
            OwlStockDbContext context = BuildDbContext();

            var photos = new List<Photo>
            {
                new Photo
                {
                    Name = "Test Photo 1",
                    Description = null
                }
            };

            await context.AddRangeAsync(photos);
            await context.SaveChangesAsync();
            return context;
        }
    }
}
