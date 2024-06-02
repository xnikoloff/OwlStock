using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot;
using OwlStock.Services;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using OwlStock.Web.DTOs.PhotoShootDTOs;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly IPhotoShootService _photoShootService;
        private readonly IPhotoService _photoService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationController(IPhotoShootService photoShootService, IPhotoService photoService, 
            IFileService fileService, IWebHostEnvironment webHostEnvironment, IEmailService emailService, UserManager<IdentityUser> userManager)
        {
            _photoShootService = photoShootService;
            _photoService = photoService;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Photoshoots()
        {
            return View(await _photoShootService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> ManagePhotoshoot(Guid id)
        {
            PhotoShoot photoshoot = await _photoShootService.PhotoShootById(id);
            ManagePhotoshootDTO dto = new()
            {
                Id = photoshoot.Id,
                CreatedOn = photoshoot.CreatedOn,
                ReservationDate = photoshoot.ReservationDate,
                PersonFullName = photoshoot.PersonFullName,
                PersonEmail = photoshoot.PersonEmail,
                PersonPhone = photoshoot.PersonPhone,
                Price = photoshoot.Price,
                PhotoShootType = photoshoot.PhotoShootType,
                PhotoShootTypeDescription = photoshoot.PhotoShootTypeDescription,
                PhotoDeliveryAddress = photoshoot.PhotoDeliveryAddress,
                GoogleMapsLink = photoshoot.GoogleMapsLink,
                PhotoDeliveryMethod = photoshoot.PhotoDeliveryMethod,
                UserPlace = photoshoot.UserPlace
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePhotoshoot(ManagePhotoshootDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _photoShootService.Update(dto);
            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFiles(Guid id)
        {
            PhotoShoot photoShootById = await _photoShootService.PhotoShootById(id);

            UpdatePhotoShootPhotosDTO filesToSPhotoShoot = new()
            {
                PersonFullName = photoShootById.PersonFullName,
                PhotoShootId = photoShootById.Id,
            };

            return View(filesToSPhotoShoot);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFiles(UpdatePhotoShootPhotosDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            string webRootPath = _webHostEnvironment.WebRootPath;

            IEnumerable<PhotoShootPhoto> photos = BuildPhotoShootPhotoList(dto.Files, new() { Id = dto.PhotoShootId, PersonFullName = dto.PersonFullName }, webRootPath);

            foreach (PhotoShootPhoto photo in photos)
            {
                bool exists = _fileService.CreatePhotoFile(photo);

                if (exists)
                {
                    ModelState.AddModelError("Files", "File already exists");
                    return View(dto);
                }

                await _photoService.Create(photo);
            }

            await _emailService.Send(new UpdatePhotoShootEmailTemplateDTO()
            {
                PersonFullName = dto.PersonFullName,
                EmailTemplate = EmailTemplate.UpdatePhotosForPhotoShoot,
                Recipient = await GetUserEmail(),
                Url = $"http:///flash-studio.co/photoshoot/{dto.PhotoShootId}/"
            });

            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpGet]
        public async Task<IActionResult> AcceptPhotoshoot(Guid id)
        {
            await _photoShootService.ChangeStatus(id, PhotoshootStatus.Accepted);
            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpGet]
        public async Task<IActionResult> DeclinePhotoshoot(Guid id)
        {
            await _photoShootService.ChangeStatus(id, PhotoshootStatus.Declined);
            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpGet]
        public async Task<IActionResult> CancelPhotoshoot(Guid id)
        {
            await _photoShootService.ChangeStatus(id, PhotoshootStatus.Cancelled);
            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpPost]
        public async Task<IActionResult> ApprovePhotoshoot(Guid id, PhotoshootStatus status)
        {
            await _photoShootService.ChangeStatus(id, status);
            return RedirectToAction(nameof(Photoshoots));
        }

        private static IEnumerable<PhotoShootPhoto> BuildPhotoShootPhotoList(IEnumerable<IFormFile> files, PhotoShoot photoShoot, string webRootPath)
        {
            List<PhotoShootPhoto> photoShootPhotos = new();

            foreach (IFormFile file in files)
            {
                MemoryStream stream = new();
                file.CopyTo(stream);

                photoShootPhotos.Add
                (
                    new()
                    {
                        FileData = stream.ToArray(),
                        FileName = file.FileName,
                        FileType = file.ContentType,
                        PhotoShoot = photoShoot,
                        FilePath = Path.Combine(webRootPath, $"resources/photoshoots/{photoShoot.PersonFullName}_{photoShoot.Id}").Replace('\\', '/')
                    }
                );
            }

            return photoShootPhotos;
        }

        private async Task<string> GetUserEmail()
        {
            var user = await _userManager.FindByIdAsync(GetUserId()) ?? throw new NullReferenceException($"User not logged in");
            return user.Email ?? throw new NullReferenceException($"{nameof(user.Email)} is null");
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException("User not logged in");
        }
    }
}
