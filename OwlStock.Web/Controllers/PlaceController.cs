using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;
using OwlStock.Web.DTOs.PlaceDTOs;

namespace OwlStock.Web.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IPlaceService _placeService;
        private readonly ISettlementService _settlementService;
        private readonly IFileService _fileService;
        private readonly IPhotoService _photoService;
        private readonly IPhotoResizer _photoResizer;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlaceController(IPlaceService placeService, ISettlementService settlementService,
            IFileService fileService, IPhotoService photoService, IPhotoResizer photoResizer, 
            IWebHostEnvironment webHostEnvironment
            ) 
        {
            _placeService = placeService;
            _settlementService = settlementService;
            _fileService = fileService;
            _photoService = photoService;
            _photoResizer = photoResizer;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View(await _placeService.All());
        }

        [HttpGet]
        public async Task<IActionResult> PlaceById(Guid id)
        {
            Place? place = await _placeService.PlaceById(id);

            if (place == null)
            {
                return View("Error", "Мястото не може да бъде намерено");
            }

            return View(place);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new PlaceDTO()
            {
                Cities = (await _settlementService.GetCitiesByServicedRegions()).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlaceDTO dto)
        {
            dto.Place.PhotoBaseId = (await CreatePlacePhoto(dto)).Id;
            Place? createdPlace = await _placeService.Create(dto.Place);

            await CreatePlacePhotoFile(createdPlace, ConvertFormFileToByteArray(dto.File));

            if (createdPlace != null)
            {
                return RedirectToAction(nameof(PlaceById), new { id = createdPlace.Id, isUpdate = false });
            }

            return View("Error", "An error occured while updating the place");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            Place? place = await _placeService.PlaceById(id);

            if (place == null)
            {
                return View("Error", "Мястото не може да бъде намерено");
            }

            return View(new PlaceDTO()
            {
                Place = place,
                Cities = (await _settlementService.GetCitiesByServicedRegions()).ToList(),
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(PlaceDTO dto)
        {
            Place? updatedPlace = await _placeService.Update(dto.Place);
            
            //update place photo if file is not null
            if (dto.File != null)
            {
                PhotoBase createdPhoto = dto.Place.PhotoBase = await CreatePlacePhoto(dto);
                await _placeService.UpdatePhotoId(updatedPlace.Id, createdPhoto.Id);
                await CreatePlacePhotoFile(dto.Place, ConvertFormFileToByteArray(dto.File));
            }
            
            if (updatedPlace != null)
            {
                return RedirectToAction(nameof(PlaceById), new{ id = updatedPlace.Id, isUpdate = false });
            }

            return View("Error", "An error occured while updating the place");
        }

        private async Task<PhotoBase> CreatePlacePhoto(PlaceDTO dto)
        {
            string resourcesPath = Path.Combine("resources", "places");

            PhotoBase photoBase = new()
            {
                FileName = dto?.File?.FileName,
                FilePath = $"{Path.Combine(resourcesPath, dto?.File?.FileName)}",
                FileType = dto.File.ContentType,
                IsDeleted = false,
            };


            return await _photoService.Create(photoBase);
        }

        private async Task CreatePlacePhotoFile(Place place, byte[] fileData)
        {
            byte[] resized = _photoResizer.Resize(fileData, PhotoSize.Small);
            place.PhotoBase.FilePath = Path.Combine(_webHostEnvironment.WebRootPath, place.PhotoBase.FilePath);
            await _fileService.CreatePlacePhotoFile(new CreatePlacePhotoFileDTO()
            {
                PlaceId = place!.Id,
                PhotoBase = place.PhotoBase,
                FileData = resized
            });
        }

        private static byte[] ConvertFormFileToByteArray(IFormFile formFile)
        {
            using var stream = new MemoryStream();
            formFile.CopyTo(stream);
            return stream.ToArray();
        }
    }
}
