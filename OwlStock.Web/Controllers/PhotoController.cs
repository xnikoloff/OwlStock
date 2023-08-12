using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;
using System.Security.Claims;
using OwlStock.Services;
using Microsoft.AspNetCore.Authorization;

namespace OwlStock.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IGalleryService _galleryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryService _categoryService;
        private readonly IPhotoTagService _photoTagService;
        private readonly IFileService _fileService;
        
        public PhotoController(IPhotoService photoService, IWebHostEnvironment webHostEnvironment,
            ICategoryService categoryService, IPhotoTagService photoTagService, IGalleryService galleryService,
             IFileService fileService)
        {
            _photoService = photoService;
            _webHostEnvironment = webHostEnvironment;
            _categoryService = categoryService;
            _photoTagService = photoTagService;
            _galleryService = galleryService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View(await _galleryService.All());
        }

        [HttpGet]
        public async Task<IActionResult> PhotoById(Guid? id)
        {
            return View(await _photoService.GetById(id));
        }

        [HttpGet]
        public async Task<IActionResult> AllByCategory(Category category)
        {
            ViewData["categoryDescription"] = _categoryService.GetCategoryDescription(category);
            return View(await _galleryService.AllByCategory(category));
        }

        [HttpGet]
        public async Task<IActionResult> AllByTag(string tag)
        {
            ViewData["categoryDescription"] = tag;
            return View("AllByCategory", await _galleryService.AllByTags(tag));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGalleryPhotoDTO? dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if(dto is not null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new NullReferenceException("User not signed in");
                
                if(dto.GalleryPhoto is null)
                {
                    throw new NullReferenceException($"{nameof(dto.GalleryPhoto)} is null");
                }

                if (dto.FormFile is null)
                {

                    return View(dto);
                }

                if (dto.Tags is null)
                {
                    return View(dto);
                }

                dto.GalleryPhoto.FileName = dto.FormFile.FileName;
                dto.GalleryPhoto.FileType = dto.FormFile.ContentType;
                dto.GalleryPhoto.FilePath = Path.Combine(webRootPath, "images");

                using MemoryStream stream = new();
                dto.FormFile.CopyTo(stream);
                dto.GalleryPhoto.FileData = stream.ToArray();
                dto.GalleryPhoto.IdentityUserId = userId;

                _fileService.CreatePhotoFile(dto.GalleryPhoto);

                Guid photoId = await _photoService.Create(dto.GalleryPhoto);
                await _categoryService.Create(dto.Categories, photoId);

                await _photoTagService.Add(dto.Tags, photoId);

            }
            
            return RedirectToAction(nameof(All));
        }
    }
}
