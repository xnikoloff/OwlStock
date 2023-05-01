using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IBraintreeService _braintreeService;

        public OrderController(IOrderService orderService, IBraintreeService braintreeService)
        {
            _orderService = orderService;
            _braintreeService = braintreeService;
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException($"User Id not available");
            
            return View(await _orderService.All(userId));
        }

        [HttpGet]
        public async Task<IActionResult> OrderInfo(PhotoByIdDTO dto)
        {
            Order order = new()
            {
                Date = DateTime.Now,
                IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Photo = dto.Photo,
                PhotoSize = dto.PhotoSize,
                Nonce = ""
            };

            if (dto.Photo.IsFree)
            {
                await _orderService.CreateOrder(order);
                return RedirectToAction(nameof(DownloadController.FreeDownload), "Download",
                    new { id = order.Id });
            }
            
            GenerateToken();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (order.Photo == null)
            {   
                return View("_Error");
            }

            if (string.IsNullOrEmpty(order.Nonce))
            {
                return View("_Error");
            }

            var gateway = _braintreeService.GetGateway();
            
            var request = new TransactionRequest
            {
                
                Amount = order.Photo.Price,
                PaymentMethodNonce = order.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                order.IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                order = await _orderService.CreateOrder(order);

                List<Category> categories = order.Photo.PhotoCategories.Select(pc => pc.Category).ToList();
                return RedirectToAction(nameof(DownloadController.DownloadPrompt),"Download", 
                    new { id = order.Id, categories });
            }

            return View("_Error");
        }

        private void GenerateToken()
        {
            //generate token
            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();
            ViewBag.ClientToken = clientToken;
        }
    }
}
