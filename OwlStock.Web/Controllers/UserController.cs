using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Services.Interfaces;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IGalleryService _galleryService;
        
        public UserController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyPhotos()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException($"User Id not available");

            return View(await _galleryService.All(userId));
        }
    }
}
