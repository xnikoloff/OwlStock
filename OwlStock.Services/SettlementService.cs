using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.Common.HelperClasses.Weather;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class SettlementService : ISettlementService
    {
        private readonly string _apiKey;

        private readonly OwlStockDbContext _context;
        private readonly IConfiguration _configuration;

        public SettlementService(OwlStockDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _apiKey = configuration.GetSection("WeatherStack").GetSection("Key").Value!;
        }

        public async Task<IEnumerable<City>> Autocomplete(string query)
        {
            if(_context.Cities is null)
            {
                throw new NullReferenceException($"{nameof(_context.Cities)} is null");
            }

            return await _context.Cities.Where(c => c.Name.Contains(query)).ToListAsync();
        }

        public async Task<City> GetCityById(int id)
        {
            if(_context.Cities is null)
            {
                throw new NullReferenceException($"{nameof(_context.Cities)} is null");
            }

            if(id == 0)
            {
                throw new NullReferenceException($"{nameof(id)} is 0");
            }

            return await _context.Cities
                .Include(c => c.Region)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync() ?? 
                throw new NullReferenceException($"City with {nameof(id)} {id} cannot be found");
        }

        public async Task<IEnumerable<Region>> GetServicedRegion()
        {
            if (_context.Regions is null)
            {
                throw new NullReferenceException($"{nameof(_context.Cities)} is null");
            }

            return await _context.Regions
                .Where(r => r.Name.Equals("Пловдив") ||
                r.Name.Equals("Пазарджик") ||
                r.Name.Equals("Хасково") ||
                r.Name.Equals("Стара Загора"))
                .ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesByServicedRegions()
        {
            if (_context.Cities is null)
            {
                throw new NullReferenceException($"{nameof(_context.Cities)} is null");
            }

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

        public async Task<IEnumerable<City>> GetCitiesByRegion(string region)
        {
            if (_context.Cities is null)
            {
                throw new NullReferenceException($"{nameof(_context.Cities)} is null");
            }
            var cities = await _context.Cities.Include(c => c.Region).ToListAsync();

            var result = await _context.Cities
                .Include(c => c.Region)
                .Where(c => c.Region.Name.Equals(region))
                .ToArrayAsync();
            return result;
        }

        public async Task<double[]> GetLatitudeAndLongitude(int settlementId)
        {
            if (_context.Cities is null)
            {
                throw new NullReferenceException($"{nameof(_context.Cities)} is null");
            }

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
    }
}
