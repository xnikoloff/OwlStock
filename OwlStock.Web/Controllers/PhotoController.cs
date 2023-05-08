using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;
using System.Security.Claims;
using OwlStock.Domain.Entities;

namespace OwlStock.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IPhotoResizer _photoResizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryService _categoryService;
        
        public PhotoController(IPhotoService photoService, IPhotoResizer photoResizer, IWebHostEnvironment webHostEnvironment,
            ICategoryService categoryService)
        {
            _photoService = photoService;
            _photoResizer = photoResizer;
            _webHostEnvironment = webHostEnvironment;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View(await _photoService.All());
        }

        [HttpGet]
        public async Task<IActionResult> PhotoById(int? id)
        {
            return View(await _photoService.GetById(id));
        }

        [HttpGet]
        public async Task<IActionResult> AllByCategory(Category category)
        {
            ViewData["categoryDescription"] = GetCategoryDescription(category);
            return View(await _photoService.AllByCategory(category));
        }

        [HttpGet]
        public async Task<IActionResult> AllByTag(string tag)
        {
            return View("AllByCategory", await _photoService.AllByTags(tag));
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
                await _photoService.Create(createPhotoDTO);
            }
            
            return RedirectToAction(nameof(All));
        }

        private string GetCategoryDescription(Category category)
        {
            return _categoryService.GetCategoryDescription(category);
        }
    }
}
