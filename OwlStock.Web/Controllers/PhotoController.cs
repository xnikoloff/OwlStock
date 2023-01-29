using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotoController(IPhotoService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
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
    }
}
