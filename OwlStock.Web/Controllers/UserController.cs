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
        private readonly IOrderService _orderService;

        public UserController(IPhotoService photoService, IOrderService orderService)
        {
            _photoService = photoService;
            _orderService = orderService;
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

            return View(await _photoService.All(userId));
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException($"User Id not available");



            return View(await _orderService.All(userId));
        }
    }
}
