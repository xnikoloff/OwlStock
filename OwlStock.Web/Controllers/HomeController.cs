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
        
        public HomeController(IGalleryService galleryService, IHomeService homeService)
        {
            _galleryService = galleryService;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HomePhoto = await _homeService.ChooseHomePagePhoto();
            List<GalleryPhoto> galleryPhotos = await _galleryService.All();
            
            return View(galleryPhotos);
        }
    }
}
