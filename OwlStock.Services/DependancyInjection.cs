using Microsoft.Extensions.DependencyInjection;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IPhotoResizer, PhotoResizer>();
            services.AddTransient<IOrderService, OrderService>();

            return services;
        }
    }
}
