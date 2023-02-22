using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using OwlStock.Domain.Entities;

namespace OwlStock.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _service;
        private readonly IPhotoResizer _photoResizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public PhotoController(IPhotoService service, IPhotoResizer photoResizer, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _photoResizer = photoResizer;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View(await _service.All());
        }

        [HttpGet]
        public async Task<IActionResult> PhotoById(int? id)
        {
            return View(await _service.GetById(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePhotoDTO? createPhotoDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(createPhotoDTO);
            }

            if(createPhotoDTO is not null)
            {
                createPhotoDTO.WebRootPath = _webHostEnvironment.WebRootPath;
                createPhotoDTO.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _service.Create(createPhotoDTO);
            }
            
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public FileResult DownloadPhoto(Photo photo, PhotoSize photoSize)
        {

            byte[] fileData = System.IO.File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\images\\{PhotoSize.OriginalSize.ToString() + "_" + photo?.FileName}");
            byte[] resized = _photoResizer.Resize(fileData, photoSize);

            if (!string.IsNullOrEmpty(photo?.FileType))
            {
                return File(resized, photo.FileType, photo?.FileName);
            }

            throw new NullReferenceException($"{nameof(photo.FileType)} is null");
        }
    }
}
