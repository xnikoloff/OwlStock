using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;
using OwlStock.Web.DTOs.PlaceDTOs;
using System.Transactions;

namespace OwlStock.Web.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IPlaceService _placeService;
        private readonly ISettlementService _settlementService;
        private readonly IFileService _fileService;
        private readonly IPhotoService _photoService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlaceController(IPlaceService placeService, ISettlementService settlementService,
            IFileService fileService, IPhotoService photoService, IWebHostEnvironment webHostEnvironment) 
        {
            _placeService = placeService;
            _settlementService = settlementService;
            _fileService = fileService;
            _photoService = photoService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View(await _placeService.All());
        }

        [HttpGet]
        public async Task<IActionResult> PlaceById(Guid id, bool isUpdate)
        {
            Place? place = await _placeService.PlaceById(id);

            if (place == null)
            {
                return View("Error", "Мястото не може да бъде намерено");
            }

            ViewData["Title"] = place?.Name;

            if (isUpdate)
            {
                return View("Update", place);
            }

            return View(place);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CreatePlaceDTO()
            {
                Cities = (await _settlementService.GetCitiesByServicedRegions()).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePlaceDTO dto)
        {
            string resourcesPath = Path.Combine("resources", "places");

            PhotoBase photoBase = new()
            {
                FileName = dto?.File?.FileName,
                FilePath = $"{Path.Combine(_webHostEnvironment.WebRootPath, resourcesPath, dto?.File?.FileName)}",
                FileType = dto.File.ContentType,
                IsDeleted = false,
            };


            Guid photoId = await _photoService.Create(photoBase);
            dto.Place.PhotoBaseId = photoId;
            Place? createdPlace = await _placeService.Create(dto.Place);

            await _fileService.CreatePlacePhotoFile(new CreatePlacePhotoFileDTO()
            {
                PlaceId = createdPlace!.Id,
                PhotoBase = createdPlace.PhotoBase,
                File = dto.File
            });

            if (createdPlace != null)
            {
                return RedirectToAction(nameof(PlaceById), new { id = createdPlace.Id, isUpdate = false });
            }

            return View("Error", "An error occured while updating the place");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Place place)
        {
            Place? updatedPlace = await _placeService.Update(place);

            if (updatedPlace != null)
            {
                return RedirectToAction(nameof(PlaceById), new{ id = updatedPlace.Id, isUpdate = false });
            }

            return View("Error", "An error occured while updating the place");
        }
    }
}
