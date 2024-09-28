using OwlStock.Domain.Entities;
using OwlStock.Services.Common.HelperClasses.Weather;

namespace OwlStock.Services.Interfaces
{
    public interface ISettlementService
    {
        Task<IEnumerable<City>> Autocomplete(string query);
        Task<City> GetCityById(int id);
        Task<IEnumerable<Region>> GetServicedRegion();
        Task<IEnumerable<City>> GetCitiesByRegion(string region);
        Task<IEnumerable<City>> GetCitiesByServicedRegions();
        Task<IEnumerable<SettlementInfo>> GetSettlementInfo(string settlement);
        Task<string> GetPopularPlaceSettlementName(Guid placeId);
        Task<double[]> GetLatitudeAndLongitude(int settlementId);
    }
}
