using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

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
                await _service.Create(createPhotoDTO);
            }
            
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public FileResult DownloadPhoto(Photo photo, PhotoSize photoSize)
        {
            if(string.IsNullOrEmpty(photo?.FileName) || string.IsNullOrEmpty(photo?.FileType) || 
                photo?.FileData == null)
            {
                throw new ArgumentNullException($"{nameof(photo)}");
            }

            Photo resizedPhoto = _photoResizer.Resize(photo, photoSize);

            if (string.IsNullOrEmpty(resizedPhoto?.FileName) || string.IsNullOrEmpty(resizedPhoto?.FileType) ||
                resizedPhoto?.FileData == null)
            {
                throw new NullReferenceException($"{nameof(photo)} null");
            }

            return File(resizedPhoto.FileData, resizedPhoto.FileType, resizedPhoto.FileName);
        }
    }
}
