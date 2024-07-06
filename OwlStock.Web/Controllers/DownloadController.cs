using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Interfaces;
using System.IO.Compression;

namespace OwlStock.Web.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IPhotoResizer _photoResizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOrderService _orderService;
        private readonly IPhotoShootService _photoShootService;
        
        public DownloadController(IPhotoService photoService, IPhotoResizer photoResizer, IWebHostEnvironment webHostEnvironment, IOrderService orderService, IPhotoShootService photoShootService)
        {
            _photoService = photoService;
            _photoResizer = photoResizer;
            _webHostEnvironment = webHostEnvironment;
            _orderService = orderService;
            _photoShootService = photoShootService;
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

        [HttpGet]
        public async Task<IActionResult> DownloadAll(Guid photoshootId)
        {
            string photoshootPersonName = await _photoShootService.GetPersonName(photoshootId);
            string photoshootFolderName = $"{photoshootPersonName}_{photoshootId}";
            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "resources", "photoshoots", photoshootFolderName);
            string[] files = Directory.GetFiles(folderPath);

            if (files.Any())
            {
                using MemoryStream memoryStream = new();
                using (ZipArchive zipArchive = new(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new(file);
                        ZipArchiveEntry entry = zipArchive.CreateEntry(fileInfo.Name);

                        using Stream entryStream = entry.Open();
                        using FileStream fileStream = new(file, FileMode.Open, FileAccess.Read);
                            
                        fileStream.CopyTo(entryStream);
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(), "application/zip", "album.zip");
            }


            return StatusCode(500, "An error cccured while zipping files");
        }
    }
}
