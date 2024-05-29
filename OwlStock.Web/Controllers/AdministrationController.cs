using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly IPhotoShootService _photoShootService;

        public AdministrationController(IPhotoShootService photoShootService)
        {
            _photoShootService = photoShootService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Photoshoots()
        {
            return View(await _photoShootService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> ManagePhotoshoot(Guid id)
        {
            return View(await _photoShootService.PhotoShootById(id));
        }
    }
}
