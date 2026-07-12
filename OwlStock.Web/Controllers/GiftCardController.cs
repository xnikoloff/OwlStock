using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    [Route("podarachni-vaucheri")]
    public class GiftCardController : Controller
    {
        private readonly IGiftCardService _giftCardService;

        public GiftCardController(IGiftCardService giftCardService)
        {
            _giftCardService = giftCardService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("vaucher")]
        public async Task<IActionResult> GiftCardById(Guid id)
        {
            return View(await _giftCardService.GetById(id));
        }

        [HttpGet("nov-vaucher")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("nov-vaucher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGiftCardDTO dto)
        {
            bool result = await _giftCardService.Create(dto.GiftCard ?? new());

            if (result)
            {
                return View(nameof(Template), dto.GiftCard ?? new());
            }

            else
            {
                return View("Error", "Неуспешно създаване на ваучер");
            }
        }

        [HttpGet("template")]
        public IActionResult Template(GiftCard giftCard)
        {
            return View(giftCard);
        }
    }
}
