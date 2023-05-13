using Microsoft.AspNetCore.Mvc;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    public class PhotoShootController : Controller
    {
        private readonly IPhotoShootService _photoShootService;

        public PhotoShootController(IPhotoShootService photoShootService)
        {
            _photoShootService = photoShootService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Reserve()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(CreatePhotoShootDTO dto)
        {
            dto.IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _photoShootService.Reserve(dto);
            return View("_SucessfulReservation");
        }

        [HttpGet]
        public async Task<IActionResult> MyPhotoShoots()
        {
            List<MyPhotoShootsDTO> myPhotoShoots = await _photoShootService.MyPhotoShoots(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            return View(myPhotoShoots);
        }
    }
}
