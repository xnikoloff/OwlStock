using Microsoft.AspNetCore.Mvc;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IHomeService _homeService;

        public HomeController(IPhotoService photoService, IHomeService homeService)
        {
            _photoService = photoService;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HomePhoto = await _homeService.ChooseHomePagePhoto();
            List<AllPhotosDTO> dto = await _photoService.All();
            return View(dto.TakeLast(12).ToList());
        }
    }
}
