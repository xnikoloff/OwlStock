using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.DTOs.Testimonies;
using OwlStock.Services.Facades.Interfaces;
using OwlStock.Services.Interfaces;
using OwlStock.Web.DTOs.Identity;
using OwlStock.Web.DTOs.PhotoShootDTOs;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Authorize(Roles = "Administrator")]

    public class AdministrationController : Controller
    {
        private readonly IAdministrationService _administrationService;
        private readonly IPhotoShootService _photoShootService;
        private readonly IGiftCardService _giftCardService;
        private readonly IPhotoService _photoService;
        private readonly IGalleryService _galleryService;
        private readonly IFileService _fileService;
        private readonly ITestimonyService _testimonyService;
        private readonly IAnnouncementService _announcementService;
        private readonly IPhotoshootFacade _photoshootFacade;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController
        (
            IAdministrationService administrationService, 
            IPhotoShootService photoShootService, 
            IGiftCardService giftCardService, 
            IPhotoService photoService, 
            IGalleryService galleryService, 
            IFileService fileService, 
            IPhotoshootFacade photoshootFacade, 
            IWebHostEnvironment webHostEnvironment, 
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            ITestimonyService testimonyService, 
            IAnnouncementService announcementService
        )
        {
            _administrationService = administrationService;
            _photoShootService = photoShootService;
            _giftCardService = giftCardService;
            _photoService = photoService;
            _galleryService = galleryService;
            _fileService = fileService;
            _testimonyService = testimonyService;
            _announcementService = announcementService;
            _photoshootFacade = photoshootFacade;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePhotoshoot(Guid id)
        {
            UpdatePhotoShootDTO dto = await _photoshootFacade.GetDataForUpdate(id);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhotoshoot(UpdatePhotoShootDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            bool result = await _photoshootFacade.UpdatePhotoshoot(dto);

            if(!result)
            {
                return View("Error", "Error while updating photoshoot");
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Photoshoots()
        {
            IEnumerable<PhotoShoot> photoShoots = await _photoShootService.GetAll();    

            if(photoShoots == null)
            {
                return View("Error", "Нещо се обърка при извличането на Вашите фотосесии");
            }

            return View(photoShoots);
        }

        [HttpGet]
        public async Task<IActionResult> PhotoshootDates()
        {
            return View(await _photoshootFacade.GetCalendarWithStatus());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ServiceReservation(DateTime date)
        {
            bool success = await _photoShootService.SetReservedDate(date);
            if (success)
            {
                return RedirectToAction(nameof(PhotoshootDates));
            }
            else
            {
                return View("Error", "Неуспешна служебна резервация");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManagePhotoshoot(Guid id)
        {
            PhotoShoot? photoshoot = await _photoShootService.PhotoShootById(id);
            
            if(photoshoot.Id == Guid.Empty)
            {
                return View("Error", "Фотосесията не е намерена");
            }

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
                //GoogleMapsLink = photoshoot.GoogleMapsLink,
                PhotoDeliveryMethod = photoshoot.PhotoDeliveryMethod,
                Place = photoshoot?.Place?.Name,
                City = photoshoot?.Place?.City?.Name,
                Region = photoshoot?.Place?.City?.Region?.Name,
                Transport = photoshoot!.TransportCustomer,
                PickUpAddress = photoshoot?.PickUpAddress,
                Status = photoshoot?.Status,
                PhotoShootPhotos = photoshoot?.PhotoShootPhotos.ToList() ?? new()

            };
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFiles(Guid id)
        {
            PhotoShoot photoShootById = await _photoShootService.PhotoShootById(id);

            if(photoShootById.Id == Guid.Empty)
            {
                return View("Error", "Фотосесията не е намерена");
            }

            UpdatePhotoShootPhotosDTO filesToSPhotoShoot = new()
            {
                PersonFirstName = photoShootById.PersonFirstName,
                PersonLastName = photoShootById.PersonLastName,
                PersonFullName = photoShootById.PersonFullName,
                PhotoShootId = photoShootById.Id,
            };

            return View(filesToSPhotoShoot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFiles(UpdatePhotoShootPhotosDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            string webRootPath = _webHostEnvironment.WebRootPath;

            IEnumerable<PhotoShootPhoto> photos = BuildPhotoShootPhotoList(dto.Files, new() { Id = dto.PhotoShootId, PersonFirstName = dto.PersonFirstName, PersonLastName = dto.PersonLastName }, webRootPath);

            foreach (PhotoShootPhoto photo in photos)
            {
                bool result = _fileService.CreatePhotoFile(photo);

                if (!result)
                {
                    ModelState.AddModelError("Files", "Някой от файловете не беше добавен. Провери лога за повече детайли.");
                    return View(dto);
                }

                await _photoService.Create(photo, GetUserId());
            }
            
            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpGet]
        public async Task<IActionResult> DeclinePhotoshoot(Guid id)
        {
            await _photoshootFacade.ChangeStatus(id, PhotoshootStatus.Declined);
            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpGet]
        public async Task<IActionResult> CancelPhotoshoot(Guid id)
        {
            await _photoshootFacade.ChangeStatus(id, PhotoshootStatus.Cancelled);
            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpGet]
        public async Task<IActionResult> CompletePhotoshoot(Guid id)
        {
            bool isSuccessful = await _photoshootFacade.ChangeStatus(id, PhotoshootStatus.Completed);
            return RedirectToAction(nameof(Photoshoots));
        }

        [HttpGet]
        public async Task<IActionResult> GiftCards()
        {
            return View(await _giftCardService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> ManageGiftCard(Guid id)
        {
            return View(await _giftCardService.GetById(id));
        }

        [HttpPost()]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineGiftCard(Guid id)
        {
            bool result = await _giftCardService.ChangeStatus(id, GiftCardStatus.Declined);

            if (result)
            {
                return RedirectToAction(nameof(GiftCards));
            }
            else
            {
                return View("Error", "Получи се грешка по време на смяната на статус на подаръчния ваучер");
            }
        }

        [HttpPost()]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateGiftCard(Guid id)
        {
            bool result = await _giftCardService.ChangeStatus(id, GiftCardStatus.Validated);

            if (result)
            {
                return RedirectToAction(nameof(GiftCards));
            }
            else
            {
                return View("Error", "Получи се грешка по време на смяната на статус на подаръчния ваучер");
            }
        }

        [HttpPost()]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetGiftCardAsUsed(Guid id)
        {
            bool result = await _giftCardService.ChangeStatus(id, GiftCardStatus.Used);

            if (result)
            {
                return RedirectToAction(nameof(GiftCards));
            }
            else
            {
                return View("Error", "Получи се грешка по време на смяната на статус на подаръчния ваучер");
            }
        }

        [HttpGet]
        public IActionResult PhotosIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Photos()
        {
            return View(await _galleryService.All());
        }

        [HttpGet]
        public IActionResult AllUsers()
        {
            return View(_userManager.Users.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> UserById(string id)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return View("Error", "User does not exists");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(IdentityUser user)
        {
            if (user.Id.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Id is empty");
                return View(nameof(UserById), user);
            }

            if (user.Email.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Email is required");
                return View(nameof(UserById), user);
            }

            IEnumerable<IdentityUser> users = await _userManager.Users.ToListAsync();

            foreach (IdentityUser existingUser in users)
            {
                if (user!.Email!.Equals(existingUser.Email)){
                    ModelState.AddModelError("", "Email is already taken");
                    return View(nameof(UserById), user);
                }
            }

            if (user.UserName.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Username is required");
                return View(nameof(UserById), user);
            }

            IdentityUser? userToUpdate = await _userManager.FindByIdAsync(user.Id);

            if (userToUpdate == null)
            {
                return View("Error", "User does not exists");
            }

            userToUpdate.UserName = user.UserName;
            userToUpdate.Email = user.Email;

            IdentityResult result = await _userManager.UpdateAsync(userToUpdate);
            
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AllUsers));
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(nameof(UserById), user);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            IdentityRole role = new()
            {
                Name = dto.Name,
                NormalizedName = dto?.Name?.ToUpper()
            };

            IdentityResult result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index), "Administration");
            }

            return View(role);

        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserRole(string id)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return View("Error", "User does not exists");
            }

            IEnumerable<string> currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles == null)
            {
                return View("Error", "An error occured while getting user's roles");
            }

            ChangeUserRoleDTO dto = new()
            {
                UserId = id,
                UserEmail = user.Email,
                Roles = _roleManager.Roles,
                SelectedRole = currentRoles.FirstOrDefault()
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleDTO dto)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(dto?.UserId ?? "");

            if (user == null)
            {
                return View("Error", "User does not exists");
            }

            IEnumerable<string> currentRoles = await _userManager.GetRolesAsync(user);

            if(currentRoles == null)
            {
                return View("Error", "An error occured while getting user's roles");
            }


            //remove current roles, if user has any
            if (currentRoles.Any())
            {
                IdentityResult removeResult = await _userManager.RemoveFromRoleAsync(user, currentRoles.FirstOrDefault() ?? "");

                if (!removeResult.Succeeded)
                {
                    foreach (IdentityError error in removeResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(dto);
                }
            }
            
            //assign new role
            IdentityResult addResult = await _userManager.AddToRoleAsync(user, dto?.SelectedRole ?? "");

            if (!addResult.Succeeded)
            {
                foreach (IdentityError error in addResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(dto);
            }

            return RedirectToAction(nameof(AllUsers));
        }

        [HttpGet]
        public async Task<IActionResult> ManageTestimonies()
        {
            ManageTestimonyDTO dto = new()
            {
                NewTestimonies = await _testimonyService.GetNew(),
                ApprovedTestimonies = await _testimonyService.GetApproved(),
                HiddenTestimonies = await _testimonyService.GetHidden(),
                UnhiddenTestimonies = await _testimonyService.GetUnhidden()
            };

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> ApproveTestimony(Guid id)
        {
            await _testimonyService.Approve(id);
            return RedirectToAction(nameof(ManageTestimonies));
        }

        [HttpGet]
        public async Task<IActionResult> HideTestimony(Guid id)
        {
            await _testimonyService.Hide(id);
            return RedirectToAction(nameof(ManageTestimonies));
        }

        [HttpGet]
        public async Task<IActionResult> UnhideTestimony(Guid id)
        {
            await _testimonyService.Unhide(id);
            return RedirectToAction(nameof(ManageTestimonies));
        }

        [HttpGet]
        public async Task<IActionResult> ManageAnnouncements()
        {
            IEnumerable<Announcement> announcements = await _announcementService.GetAll();
            return View(announcements);
        }

        [HttpGet]
        public IActionResult CreateAnnouncement()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return View(announcement);
            }

            bool result = await _announcementService.Create(announcement, GetUserId());

            if (!result)
            {
                return View("Error", "Неуспешно създаване на новина");
            }

            return RedirectToAction(nameof(ManageAnnouncements));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAnnouncement(Guid id)
        {
            Announcement? announcement = await _announcementService.GetById(id);

            if (announcement.Id == Guid.Empty)
            {
                return View("Error", "Новината не е намерена");
            }

            return View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAnnouncement(Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return View(announcement);
            }

            bool result = await _announcementService.Update(announcement, GetUserId());

            if (!result)
            {
                return View("Error", "Неуспешно обновяване на новина");
            }

            return RedirectToAction(nameof(ManageAnnouncements));
        }

        [HttpGet]
        public async Task<IActionResult> ManageAnnouncementsVisibility(Guid id)
        {
            bool result = await _announcementService.ManageAnnouncementsVisibility(id, GetUserId());

            if (!result)
            {
                return View("Error", "Неуспешна промяна на видимостта на новината");
            }

            return RedirectToAction(nameof(ManageAnnouncements));
        }

        [HttpGet]
        public IActionResult ManageWorkingTimeIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageWorkingTime(WorkingTimeType workingTimeType)
        {
            return View(await _administrationService.GetWorkingTimeByType(workingTimeType));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ManageWorkingTime(WorkingTime workingTime)
        {
            if (!ModelState.IsValid)
            {
                return View(workingTime);
            }

            if (workingTime.Start > workingTime.End)
            {
                ModelState.AddModelError("", "Началният час трябва да е по-рано от крайния час");
                return View(workingTime);
            }

            if (workingTime.Start < 6 || workingTime.End > 20)
            {
                ModelState.AddModelError("", "Работното време трябва да е в интервала [6, 20]");
                return View(workingTime);
            }

            if (workingTime.Start % 1 != 0 || workingTime.End % 1 != 0)
            {
                ModelState.AddModelError("", "Работното време трябва да е цяло число");
                return View(workingTime);
            }

            bool result = await _administrationService.SetWorkingTime(workingTime);

            if (!result)
            {
                return View("Error", "Неуспешно обновяване на работното време");
            }

            return RedirectToAction(nameof(Index));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException("User not logged in");
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
                        FilePath = Path.Combine(webRootPath, $"resources/photoshoots/{photoShoot.PersonFirstName}{photoShoot.PersonLastName}_{photoShoot.Id}").Replace('\\', '/')
                    }
                );
            }

            return photoShootPhotos;
        }
    }
}
