using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IPhotoResizer _photoResizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOrderService _orderService;
        private readonly IPhotoService _photoService;

        public DownloadController(IPhotoResizer photoResizer, IWebHostEnvironment webHostEnvironment, IOrderService orderService, IPhotoService photoService)
        {
            _photoResizer = photoResizer;
            _webHostEnvironment = webHostEnvironment;
            _orderService = orderService;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPrompt(Guid id, List<Category> categories)
        {
            ViewData["categories"] = categories;
            return View(id);
        }

        public async Task<FileResult> FreeDownload(Guid id)
        {
            return await Download(id);
        }

        [HttpPost]
        public async Task<FileResult> Download(Guid id)
        {
            Order order = await _orderService.GetById(id);

            byte[] fileData = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\images\\{PhotoSize.OriginalSize.ToString() + "_" + order.Photo?.FileName}");
            byte[] resized = _photoResizer.Resize(fileData, order.PhotoSize);

            if (!string.IsNullOrEmpty(order.Photo?.FileType))
            {
                return File(resized, order.Photo.FileType, order.Photo?.FileName);
            }

            throw new NullReferenceException($"{nameof(order.Photo.FileType)} is null");
        }
    }
}
