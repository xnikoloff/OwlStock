using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;
using System.Security.Claims;
using OwlStock.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

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
        private readonly ICommonServices _commonServices;
        
        public PhotoController(IPhotoService photoService, IWebHostEnvironment webHostEnvironment,
            ICategoryService categoryService, IPhotoTagService photoTagService, IGalleryService galleryService,
             IFileService fileService, ICommonServices commonServices)
        {
            _photoService = photoService;
            _webHostEnvironment = webHostEnvironment;
            _categoryService = categoryService;
            _photoTagService = photoTagService;
            _galleryService = galleryService;
            _fileService = fileService;
            _commonServices = commonServices;
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
            ViewData["categoryDescription"] = _commonServices.GetEnumDescription(category);
            return View(await _galleryService.AllByCategory(category));
        }

        [HttpGet]
        public async Task<IActionResult> AllByPhotoshootType(PhotoShootType photoshootType)
        {
            ViewData["categoryDescription"] = _commonServices.GetEnumDescription(photoshootType);
            return View(await _galleryService.AllByPhotoshootType(photoshootType));
        }

        [HttpGet]
        public async Task<IActionResult> AllByTag(string tag)
        {
            ViewData["categoryDescription"] = tag;
            return View("AllByCategory", await _galleryService.AllByTags(tag));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
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
                dto.GalleryPhoto.FilePath = Path.Combine(webRootPath, "resources/gallery-photos").Replace('\\', '/');

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

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(PhotoByIdDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(PhotoById), new { dto?.Photo?.Id });
            }

            if (dto is not null)
            {
                if (dto.Photo is null)
                {
                    throw new NullReferenceException($"{nameof(dto.Photo)} is null");
                }

                await _photoService.Delete(dto.Photo);

            }

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ChangeDownloadPermissions(PhotoByIdDTO dto)
        {
            if(dto.Photo == null)
            {
                //return to All() if there is a problem with the model
                return await All();
            }

            if (dto.Photo.Id == default)
            {
                ModelState.AddModelError(string.Empty, "Incorrect Id");
                return await PhotoById(dto?.Photo?.Id);
            }

            Guid changePhotoId = await _photoService.ChangeDownloadPermissions(dto.Photo.Id);

            if (changePhotoId.ToString().Equals(dto?.Photo?.Id.ToString()))
            {
                return RedirectToAction(nameof(PhotoById), new { id = dto?.Photo?.Id });
            }

            return await All();
        }
    }
}
