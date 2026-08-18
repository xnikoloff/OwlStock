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
        private readonly IPdfService _pdfService;

        public GiftCardController(IGiftCardService giftCardService, IPdfService pdfService)
        {
            _giftCardService = giftCardService;
            _pdfService = pdfService;
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
        public IActionResult Template()
        {
            return View();
        }

        [HttpPost("nov-vaucher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGiftCardDTO dto)
        {
            GiftCard giftCard = await _giftCardService.Create(dto.GiftCard ?? new());
            string html = _giftCardService.GetHtmlTemplate(giftCard);
            byte[]? pdf = _pdfService.GeneratePdfFromHtml(html);

            if (pdf != null)
            {
                return DownloadGiftCard(pdf);
            }

            else
            {
                return View("Error", "Неуспешно създаване на ваучер");
            }
        }

        private FileResult DownloadGiftCard(byte[] bytes)
        {
            return File(bytes, "application/pdf", "ваучер.pdf");
        }
    }
}
