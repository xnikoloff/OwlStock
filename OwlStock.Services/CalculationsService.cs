using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure;
using OwlStock.Services.Common;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class CalculationsService : ICalculationsService
    {
        private readonly OwlStockDbContext _context;
        
        public CalculationsService(OwlStockDbContext context)
        {
            _context = context;
        }

        public decimal CalculatePhotoshootPrice(PhotoShootType type, decimal fuelPrice = 0)
        {
            switch (type)
            {
                case PhotoShootType.Personal:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.PortrairPhotoShoot;
                    }

                    return DefaultValue.PortrairPhotoShoot + fuelPrice;
                }

                case PhotoShootType.Wedding:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.WeddingPhotoshoot;
                    }

                    return DefaultValue.WeddingPhotoshoot + fuelPrice;
                }

                case PhotoShootType.Prom:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.PromPhotoshoot;
                    }

                    return DefaultValue.PromPhotoshoot + fuelPrice;
                }

                case PhotoShootType.Kids:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.KidsPhotoshoot;
                    }

                    return DefaultValue.KidsPhotoshoot + fuelPrice;
                }

                case PhotoShootType.Automotive:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.Automotive;
                    }

                    return DefaultValue.Automotive + fuelPrice;
                }

                default: return fuelPrice;
            }
        }

        public async Task<decimal> CalculateFuelPrice(string[] data)
        {
            if (_context.Regions is null)
            {
                throw new NullReferenceException($"{nameof(_context.Regions)} is null");
            }

            if (_context.Cities is null)
            {
                throw new NullReferenceException($"{nameof(_context.Cities)} is null");
            }

            int regionId = await _context.Regions
                .Where(r => r.Name!.Equals(data[0]))
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

            City? city = await _context.Cities.Where(c => c.RegionId == regionId && c.NameLatin!.Equals(data[1])).FirstOrDefaultAsync() ??
                throw new NullReferenceException($"{nameof(city)} with RegionId {regionId} and name ${data[1]} cannot be found");

            double distance = CalculateDistance(city.Latitude, city.Longitude, DefaultValue.DefaultSettlementLatitude, DefaultValue.DefaultSettlementLongitude);
            return CalculatePriceByDistance(Math.Ceiling(distance));
        }

        public double CalculateDistance(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
        {
            var d1 = latitudeA * (Math.PI / 180.0);
            var num1 = longitudeA * (Math.PI / 180.0);
            var d2 = latitudeB * (Math.PI / 180.0);
            var num2 = longitudeB * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                        Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return Math.Ceiling(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000);

        }

        public decimal CalculatePriceByDistance(double distance)
        {
            if (distance < 20)
            {
                return 0;
            }

            return Math.Ceiling(DefaultValue.TripTax + (DefaultValue.FuelPriceByKilometer * Convert.ToDecimal(distance)));
        }

        //not used for now
        /*public double CalculateTimeForTravel(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
        {
            double distance = CalculateDistance(latitudeA, longitudeA, latitudeB, longitudeB);

            //divide distance by speed to calculate the time needed
            //to travel that distance
            //double the distance to include the time needed to go back to point A
            return Math.Round(distance / DefaultValue.Speed) * 2;
        }*/
    }
}
