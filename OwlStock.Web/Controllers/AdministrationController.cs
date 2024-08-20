using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot;
using OwlStock.Services;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using OwlStock.Web.DTOs.Identity;
using OwlStock.Web.DTOs.PhotoShootDTOs;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Authorize(Roles = "Administrator")]

    public class AdministrationController : Controller
    {
        private readonly IPhotoShootService _photoShootService;
        private readonly IPhotoService _photoService;
        private readonly IGalleryService _galleryService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController(IPhotoShootService photoShootService, IPhotoService photoService, IGalleryService galleryService, 
            IFileService fileService, IWebHostEnvironment webHostEnvironment, IEmailService emailService, 
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _photoShootService = photoShootService;
            _photoService = photoService;
            _galleryService = galleryService;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
            _userManager = userManager;
            _roleManager = roleManager;
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
                PersonFirstName = photoShootById.PersonFirstName,
                PersonLastName = photoShootById.PersonLastName,
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

            IEnumerable<PhotoShootPhoto> photos = BuildPhotoShootPhotoList(dto.Files, new() { Id = dto.PhotoShootId, PersonFirstName = dto.PersonFirstName, PersonLastName = dto.PersonLastName }, webRootPath);

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

        [HttpGet]
        public async Task<IActionResult> CompletePhotoshoot(Guid id)
        {
            await _photoShootService.ChangeStatus(id, PhotoshootStatus.Completed);
            
            await _emailService.Send(new UpdatePhotoShootEmailTemplateDTO()
            {
                EmailTemplate = EmailTemplate.UpdatePhotosForPhotoShoot,
                Topic = "Страхотни новини",
                Recipient = await GetUserEmail(),
                PhotoShootId = id
            });
            
            return RedirectToAction(nameof(Photoshoots));
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
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleDTO dto)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(dto.UserId);

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
                IdentityResult removeResult = await _userManager.RemoveFromRoleAsync(user, currentRoles.FirstOrDefault());

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
            IdentityResult addResult = await _userManager.AddToRoleAsync(user, dto.SelectedRole);

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
