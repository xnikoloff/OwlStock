using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGalleryService _galleryService;
        private readonly IHomeService _homeService;
        private readonly IEmailService _emailService;

        public HomeController(IGalleryService galleryService, IHomeService homeService, IEmailService emailService)
        {
            _galleryService = galleryService;
            _homeService = homeService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HomePhoto = await _homeService.ChooseHomePagePhoto();
            List<GalleryPhoto> galleryPhotos = await _galleryService.All();
            
            return View(galleryPhotos);
        }
    }
}
