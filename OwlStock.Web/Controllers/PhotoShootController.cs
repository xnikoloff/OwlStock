using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.DTOs.PhotoShoot;
using OwlStock.Services.Interfaces;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Authorize]
    public class PhotoShootController : Controller
    {
        private readonly IPhotoShootService _photoShootService;
        private readonly ICalendarService _calendarService;
        private readonly ISettlementService _settlementService;
        private readonly IPlaceService _placeService;
        
        public PhotoShootController(IPhotoShootService photoShootService, ICalendarService calendarService, 
            ISettlementService settlementService, IPlaceService placeService)
        {
            _photoShootService = photoShootService;
            _calendarService = calendarService;
            _settlementService = settlementService;
            _placeService = placeService;
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
                RemainingDates = _calendarService.GetRemainingDates().ToList(),
                ServicedRegions = (await _settlementService.GetServicedRegion()).ToList(),
            };
            
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(CreatePhotoShootDTO dto)
        {
            if (dto.IsDecidedByUs)
            {
                ModelState.Remove("SelectedSettlementId");
            }
            if (dto.IsDecidedByUs || dto.IsPlaceSelected)
            {
                ModelState.Remove("UserPlace");
            }

            if (!ModelState.IsValid)
            {
                dto.Calendar = await _photoShootService.GetPhotoShootsCalendar();
                dto.AllTimeSlots = _calendarService.GetTimeSlots();
                dto.RemainingDates = _calendarService.GetRemainingDates().ToList();
                dto.ServicedRegions = (await _settlementService.GetServicedRegion()).ToList();
                return View(dto);
            }

            if(dto.PhotoShootType == PhotoShootType.Other && dto.PhotoShootTypeDescription.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, "Описането на фотосесията е задължително");

                dto.Calendar = await _photoShootService.GetPhotoShootsCalendar();
                dto.AllTimeSlots = _calendarService.GetTimeSlots();
                dto.RemainingDates = _calendarService.GetRemainingDates().ToList();

                return View(dto);
            }

            dto.IdentityUserId = GetUserId();
            dto.PersonEmail = User.FindFirstValue(ClaimTypes.Email);


            if (!string.IsNullOrEmpty(dto.UserPlace))
            {
                Place? place = await _placeService.Create(new()
                {
                    CityId = Convert.ToInt32(dto.SelectedSettlementId),
                    IsPopular = false,
                    Name = dto.UserPlace,
                    GoogleMapsURL = dto.GoogleMapsLink,

                });

                if (place == null)
                {
                    return View("Error", "Нещо се обърка повреме на резервирането...");
                }
            
                dto.PlaceId = place.Id;
            }
           
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
            PhotoShoot? photoshoot = await _photoShootService.PhotoShootById(id, GetUserId());
            
            if (photoshoot == null)
            {
                return View("Error", "Несъществуваща фотосесия");
            }
            return View(photoshoot);
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException("User not logged in");
        }
    }
}
