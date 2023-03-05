using Microsoft.AspNetCore.Mvc;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPhotoService _photoService;

        public HomeController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            List<AllPhotosDTO> dto = await _photoService.All();
            return View(dto.TakeLast(12).ToList());
        }
    }
}
