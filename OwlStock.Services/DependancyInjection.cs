using Microsoft.Extensions.DependencyInjection;
using OwlStock.Services.Common;
using OwlStock.Services.Facades.Implementations;
using OwlStock.Services.Facades.Interfaces;
using OwlStock.Services.Implementations;
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
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IPhotoTagService, PhotoTagService>();
            services.AddTransient<IPhotoShootService, PhotoShootService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddTransient<ISettlementService, SettlementService>();
            services.AddTransient<IWeatherService, WeatherService>();
            services.AddTransient<ICalculationsService, CalculationsService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICommonServices, CommonServices>();
            services.AddTransient<IPlaceService, PlaceService>();
            services.AddTransient<IAdministrationService, AdministrationService>();
            services.AddTransient<ITestimonyService, TestimonyService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<IGiftCardService, GiftCardService>();
            services.AddTransient<IPhotoshootFacade, PhotoshootFacade>();
            services.AddTransient<IPhotoFacade, PhotoFacade>();
            services.AddTransient<IBlogServiceFacade, BlogServiceFacade>();
            services.AddTransient<IHomeFacade, HomeFacade>();

            return services;
        }
    }
}
