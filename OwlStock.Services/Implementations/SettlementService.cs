using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.Common.HelperClasses.Weather;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class SettlementService : ISettlementService
    {
        private readonly string _apiKey;

        private readonly PhotonicDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SettlementService> _logger;

        public SettlementService(PhotonicDbContext context, IConfiguration configuration, ILogger<SettlementService> logger)
        {
            _context = context;
            _configuration = configuration;
            _apiKey = configuration.GetSection("WeatherStack").GetSection("Key").Value!;
            _logger = logger;
        }

        /// <summary>
        /// Returns list of cities that match the search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>List of City containing the matching cities</returns>
        public async Task<IEnumerable<City>> Autocomplete(string query)
        {

            return await _context.Cities.Where(c => (c.Name ?? string.Empty).Contains(query)).ToListAsync();
        }

        /// <summary>
        /// Gets a City by Id
        /// </summary>
        /// <param name="id">Id of the City</param>
        /// <returns>The City that matches the Id</returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<City> GetCityById(int id)
        {
            if(id == 0)
            {
                _logger.LogError("{id} is 0 in {Method}, {Class}, {DateTime}", nameof(id), nameof(GetCityById), nameof(SettlementService), DateTime.Now);
                return new();
            }

            return await _context.Cities
                .Include(c => c.Region)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync() ?? 
                throw new NullReferenceException($"City with {nameof(id)} {id} cannot be found");
        }

        /// <summary>
        /// Gets list of the regions that are currently serviced
        /// </summary>
        /// <returns>List of Region containing the serviced regions</returns>
        public async Task<IEnumerable<Region>> GetServicedRegion()
        {
            return await _context.Regions
                .Where(r => (r.Name ?? string.Empty).Equals("Пловдив") ||
                (r.Name ?? string.Empty).Equals("Пазарджик") ||
                (r.Name ?? string.Empty).Equals("Хасково") ||
                (r.Name ?? string.Empty).Equals("Стара Загора"))
                .ToListAsync();
        }

        /// <summary>
        /// Gets all cities for the serviced regions
        /// </summary>
        /// <returns>List of City containing the cities for the serviced regions</returns>
        public async Task<IEnumerable<City>> GetCitiesByServicedRegions()
        {

            List<City> allSettlements = await _context.Cities
                .OrderBy(c => c.Name)
                .ToListAsync();
            List<City> cities = new();
            List<Region> servicedRegions = (await GetServicedRegion()).ToList();

            foreach (Region region in servicedRegions) 
            {
                cities.AddRange(allSettlements.Where(c => c.Region == region).ToList());
            }

            return cities;
        }

        /// <summary>
        /// Get all cities for a serviced region
        /// </summary>
        /// <param name="region">Id of the region</param>
        /// <returns>List of all cities for a serviced region</returns>
        public async Task<IEnumerable<City>> GetCitiesByRegion(int region)
        {
            
            var result = await _context.Cities
                .Include(c => c.Region)
                .Where(c => c.Region.Id == region)
                .ToArrayAsync();

            return result;
        }

        /// <summary>
        /// Gets latitude and longitude of a settlement
        /// </summary>
        /// <param name="settlementId">Id of the settlement</param>
        /// <returns>An array of coordinates. 0th element is latitude, 1st element is longitude</returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<double[]> GetLatitudeAndLongitude(int settlementId)
        {
            double[]? data = await _context.Cities
                .Where(c => c.Id == settlementId)
                .Select(c => new double[] { c.Latitude, c.Longitude })
                .FirstOrDefaultAsync();

            if (data?.Length == 0 || data == null)
            {
                throw new NullReferenceException($"{nameof(City)} with Id {settlementId} cannot be found");
            }

            return data;
        }

        /// <summary>
        /// Gets all info for a settlement from the WeatherStack API which will be used as autocomplete
        /// </summary>
        /// <param name="settlement">Name of the settlement used as a keyword</param>
        /// <returns>List of SettlementInfo</returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<IEnumerable<SettlementInfo>> GetSettlementInfo(string settlement)
        {
            string? host = _configuration.GetSection("WeatherStack").GetSection("Host").Value ?? throw new NullReferenceException("Cannot get section 'Host'");
            using HttpClient client = new();
            client.BaseAddress = new Uri(host);
            string url = Path.Combine(host, _configuration.GetSection("Weatherstack").GetSection("Autocomplete").Value! + "?apikey=" + _apiKey + "&q=" + settlement);
            HttpResponseMessage response = await client.GetAsync(url);

            string json = await response.Content.ReadAsStringAsync();
            IEnumerable<SettlementInfo>? autocomplete = JsonConvert.DeserializeObject<IEnumerable<SettlementInfo>>(json);

            return autocomplete ?? throw new NullReferenceException($"{nameof(autocomplete)} is null");
        }

        /// <summary>
        /// Gets the name of the settlement in which a popular place is located in
        /// </summary>
        /// <param name="placeId">If of the popular place</param>
        /// <returns>Name of the settlement as string</returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<string> GetPopularPlaceSettlementName(Guid placeId)
        {
            if(_context.Places is null)
            {
                throw new NullReferenceException($"{nameof(_context.Places)} is null");
            }

            Place? place = await _context.Places
                .Include(p => p.City)
                .Where(p => p.Id == placeId)
                .FirstOrDefaultAsync();

            if(place?.City == null)
            {
                throw new NullReferenceException($"{nameof(place.City)} is null");
            }

            if (place.City.NameLatin.IsNullOrEmpty())
            {
                throw new NullReferenceException($"{nameof(place.City.NameLatin)} is null");
            }

            return place.City.NameLatin ?? "";
        }
    }

}
