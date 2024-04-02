using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers.API
{
    [Route("[controller]")]
    [ApiController]
    public class SettlementController : ControllerBase
    {
        private readonly ISettlementService _settlementService;

        public SettlementController(ISettlementService settlementService)
        {
            _settlementService = settlementService;
        }

        [HttpGet]
        [Route("settlementsByRegion")]
        public async Task<IEnumerable<City>> GetSettlementsByRegion(string region)
        {
            return await _settlementService.GetCitiesByRegion(region);
        }
    }
}
