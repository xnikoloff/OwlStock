﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        
        public PhotoShootController
        (
            IPhotoShootService photoShootService, ICalendarService calendarService, ISettlementService settlementService
        )
        {
            _photoShootService = photoShootService;
            _calendarService = calendarService;
            _settlementService = settlementService;
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
                ServicedRegions = (await _settlementService.GetServicedRegion()).ToList()
            };
            
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(CreatePhotoShootDTO dto)
        {
            if (dto.IsDecidedByUs)
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
                ModelState.AddModelError(string.Empty, "Provide photoshoot type description");

                dto.Calendar = await _photoShootService.GetPhotoShootsCalendar();
                dto.AllTimeSlots = _calendarService.GetTimeSlots();
                dto.RemainingDates = _calendarService.GetRemainingDates().ToList();

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

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException("User not logged in");
        }
    }
}
