using OwlStock.Domain.Entities;
using OwlStock.Services.Common.HelperClasses.Weather;

namespace OwlStock.Services.Interfaces
{
    public interface ISettlementService
    {
        Task<IEnumerable<City>> Autocomplete(string query);
        Task<IEnumerable<Region>> GetServicedRegion();
        Task<IEnumerable<City>> GetCitiesByRegion(string region);
        Task<IEnumerable<SettlementInfo>> GetSettlementInfo(string settlement);
    }
}
