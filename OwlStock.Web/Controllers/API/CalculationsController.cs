using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers.API
{
    [Route("[controller]")]
    [ApiController]
    public class CalculationsController : ControllerBase
    {
        private readonly ICalculationsService _calculationsService;
        
        public CalculationsController(ICalculationsService calculationsService)
        {
            _calculationsService = calculationsService;
        }

        [HttpGet]
        [Route("photoshootPrice")]
        public decimal CalculatePhotoshootPrice(PhotoShootType photoShootType, decimal fuelPrice)
        {
            return _calculationsService.CalculatePhotoshootPrice(photoShootType, fuelPrice);
        }

        [HttpGet]
        [Route("fuelPrice")]
        public decimal CalculateFuelPrice([FromQuery]int regionId)
        {
            return _calculationsService.CalculateFuelPrice(regionId);   
        }
    }
}
