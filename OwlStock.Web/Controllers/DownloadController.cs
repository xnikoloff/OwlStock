﻿using Microsoft.AspNetCore.Mvc;
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
        
        public DownloadController(IPhotoResizer photoResizer, IWebHostEnvironment webHostEnvironment, IOrderService orderService)
        {
            _photoResizer = photoResizer;
            _webHostEnvironment = webHostEnvironment;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPrompt(Guid id, List<Category> categories)
        {
            Order order = await _orderService.GetById(id);
            ViewData["categories"] = categories;
            return View(order.Photo);
        }

        public async Task<FileResult> FreeDownload(Guid id)
        {
            return await Download(id);
        }

        [HttpPost]
        public async Task<FileResult> Download(Guid id)
        {
            Order order = await _orderService.GetById(id);

            byte[] fileData = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\resources\\gallery-photos\\{PhotoSize.OriginalSize.ToString() + "_" + order.Photo?.FileName}");
            byte[] resized = _photoResizer.Resize(fileData, order.PhotoSize);
            
            if (!string.IsNullOrEmpty(order.Photo?.FileType))
            {
                return File(resized, order.Photo.FileType, order.Photo?.FileName);
            }

            throw new NullReferenceException($"{nameof(order.Photo.FileType)} is null");
        }

        [HttpPost]
        public FileResult DownloadPhotoShootPhoto(PhotoShootPhoto photo)
        {
            if(photo.FilePath is null)
            {
                throw new NullReferenceException($"{nameof(photo.FilePath)} is null");
            }
            byte[] fileData = System.IO.File.ReadAllBytes(Path.Combine(_webHostEnvironment.WebRootPath, "resources/" + photo.FilePath + $"/{photo.FileName}").Replace('\\', '/'));

            if (!string.IsNullOrEmpty(photo?.FileType))
            {
                return File(fileData, photo.FileType, photo?.FileName);
            }

            throw new NullReferenceException($"{nameof(photo.FileType)} is null");
        }
    }
}
