﻿using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers.API
{
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        [Route("allPopular")]
        public async Task<IEnumerable<Place>> AllPopular() 
        { 
            return await _placeService.AllPopular();
        }

        [HttpGet]
        [Route("popularPlacesByRegion")]
        public async Task<IEnumerable<Place>> PopularPlacesByRegion(int regionId)
        {
            return await _placeService.GetPopularPlacesByRegion(regionId);
        }
    }
}
