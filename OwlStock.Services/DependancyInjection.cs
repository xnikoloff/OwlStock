using Microsoft.Extensions.DependencyInjection;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IGalleryService, GalleryService>();
            services.AddTransient<IPhotoResizer, PhotoResizer>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBraintreeService, BraintreeService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IPhotoTagService, PhotoTagService>();
            services.AddTransient<IPhotoShootService, PhotoShootService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICalendarService, CalendarService>();

            return services;
        }
    }
}
