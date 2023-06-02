using Microsoft.AspNetCore.Mvc;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IHomeService _homeService;
        private readonly IEmailService _emailService;

        public HomeController(IPhotoService photoService, IHomeService homeService, IEmailService emailService)
        {
            _photoService = photoService;
            _homeService = homeService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HomePhoto = await _homeService.ChooseHomePagePhoto();
            List<AllPhotosDTO> dto = await _photoService.All();
            //await _emailService.Send();   
            return View(dto.TakeLast(12).ToList());
        }
    }
}
