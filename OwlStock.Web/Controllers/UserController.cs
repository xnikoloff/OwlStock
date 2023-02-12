using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Services.Interfaces;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IPhotoService _photoService;

        public UserController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyPhotos()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException($"User Id not available");

            return View(await _photoService.All(userId));
        }
    }
}
