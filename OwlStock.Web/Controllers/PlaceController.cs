using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService) 
        {
            _placeService = placeService;
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
