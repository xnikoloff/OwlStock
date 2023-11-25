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
    public class PhotoShootController : Controller
    {
        private readonly IPhotoShootService _photoShootService;
        private readonly IPhotoService _photoService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;
        private readonly ICalendarService _calendarService;
        private readonly UserManager<IdentityUser> _userManager;

        public PhotoShootController(IPhotoShootService photoShootService, IFileService fileService,
            IWebHostEnvironment webHostEnvironment, IPhotoService photoService, IEmailService emailService, 
            ICalendarService calendarService, UserManager<IdentityUser> userManager)
        {
            _photoShootService = photoShootService;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _photoService = photoService;
            _emailService = emailService;
            _calendarService = calendarService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reserve()
        {
            CreatePhotoShootDTO dto = new()
            {
                Calendar = await _photoShootService.GetPhotoShootsCalendar(),
                AllTimeSlots = _calendarService.GetTimeSlots(),
                RemainingDates = _calendarService.GetRemainingDates().ToList()
            };
            
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(CreatePhotoShootDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            dto.IdentityUserId = GetUserId();
            dto.PersonEmail = User.FindFirstValue(ClaimTypes.Email);

            await _photoShootService.Add(dto);
            return View("_SucessfulReservation");
        }

        [HttpGet]
        public async Task<IActionResult> MyPhotoShoots()
        {
            List<MyPhotoShootsDTO> myPhotoShoots = await _photoShootService.MyPhotoShoots(GetUserId());
            return View(myPhotoShoots);
        }

        [HttpGet]
        public async Task<IActionResult> PhotoShootById(Guid id)
        {
            return View(await _photoShootService.PhotoShootById(id));
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
            if(!ModelState.IsValid)
            {
                return View(dto);
            }

            string webRootPath = _webHostEnvironment.WebRootPath;

            IEnumerable<PhotoShootPhoto> photos = BuildPhotoShootPhotoList(dto.Files, new () { Id = dto.PhotoShootId, PersonFullName = dto.PersonFullName }, webRootPath);

            foreach(PhotoShootPhoto photo in photos)
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
                Url = $"https:///flashstudio.com/photoshoot/{dto.PhotoShootId}/"
            });
            
            return RedirectToAction(nameof(PhotoShootById), new { id = dto.PhotoShootId});
        }

        private static IEnumerable<PhotoShootPhoto> BuildPhotoShootPhotoList(IEnumerable<IFormFile> files, PhotoShoot photoShoot, string webRootPath)
        {
            List<PhotoShootPhoto> photoShootPhotos = new();

            foreach(IFormFile file in files)
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
                        FilePath = Path.Combine(webRootPath, $"images/photoshoots/{photoShoot.PersonFullName}_{photoShoot.Id}").Replace('\\', '/')
                    }
                );
            }

            return photoShootPhotos;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException("User not logged in");
        }

        private async Task<string> GetUserEmail()
        {
            
            var user = await _userManager.FindByIdAsync(GetUserId()) ?? throw new NullReferenceException($"User not logged in");
            return user.Email ?? throw new NullReferenceException($"{nameof(user.Email)} is null");

        }
    }
}
