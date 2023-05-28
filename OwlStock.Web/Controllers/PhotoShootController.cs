﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using OwlStock.Domain.Enumerations;
using OwlStock.Services;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    public class PhotoShootController : Controller
    {
        private readonly IPhotoShootService _photoShootService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotoShootController(IPhotoShootService photoShootService, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _photoShootService = photoShootService;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Reserve()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(CreatePhotoShootDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            dto.IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _photoShootService.Add(dto);
            return View("_SucessfulReservation");
        }

        [HttpGet]
        public async Task<IActionResult> MyPhotoShoots()
        {
            List<MyPhotoShootsDTO> myPhotoShoots = await _photoShootService.MyPhotoShoots(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            return View(myPhotoShoots);
        }

        [HttpGet]
        public async Task<IActionResult> PhotoShootById(Guid id)
        {
            return View(await _photoShootService.PhotoShootById(id));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            PhotoShootByIdDTO photoShootById = await _photoShootService.PhotoShootById(id);

            AddFilesToPhotoShootDTO filesToSPhotoShoot = new()
            {
                PersonFullName = photoShootById.PersonFullName,
                PhotoShootId = photoShootById.Id
            };

            return View(filesToSPhotoShoot);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFiles(AddFilesToPhotoShootDTO dto)
        {
            if(dto.FormFiles == null)
            {
                return View(dto);
            }

            _fileService.Create(dto.FormFiles, _webHostEnvironment.WebRootPath, null);
            await _fileService.CreatePhotoShootFiles(dto.FormFiles, dto.PhotoShootId, _webHostEnvironment.WebRootPath);

            return RedirectToAction(nameof(PhotoShootById), new { id = dto.PhotoShootId});
        }
    }
}
