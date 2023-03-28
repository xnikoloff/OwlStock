using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException($"User Id not available");
            
            return View(await _orderService.All(userId));
        }

        [HttpGet]
        public IActionResult OrderInfo(PhotoByIdDTO dto)
        {
            Order order = new()
            {
                Date = DateTime.Now,
                IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Photo = dto.Photo,
                PhotoSize = dto.PhotoSize
            };

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            order.IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _orderService.CreateOrder(order);
            return RedirectToAction(nameof(MyOrders));
        }
    }
}
