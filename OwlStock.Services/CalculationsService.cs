using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Common;
using OwlStock.Services.Common.Enumerations;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    public class CalculationsService : ICalculationsService
    {
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

                case PhotoShootType.WeddingPlus:
                    {
                        if (fuelPrice == 0)
                        {
                            return DefaultValue.WeddingPlusPhotoshoot;
                        }

                        return DefaultValue.WeddingPlusPhotoshoot + fuelPrice;
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

                case PhotoShootType.Pregnant:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.PregnantPhotoshoot;
                    }

                    return DefaultValue.PregnantPhotoshoot + fuelPrice;
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

        public decimal CalculateFuelPrice(int regionId)
        {
            double latitude = 0;
            double longitude = 0;

            switch (regionId)
            {
                case (int)RegionEnum.Plovdiv:
                {
                    latitude = DefaultValue.LatitudePlovdiv;
                    longitude = DefaultValue.LongitudePlovdiv;
                    break;
                }

                case (int)RegionEnum.Pazardzhik:
                {
                    latitude = DefaultValue.LatitudeHaskovo;
                    longitude = DefaultValue.LongitudeHaskovo;
                    break;
                }

                case (int)RegionEnum.StaraZagora:
                {
                    latitude = DefaultValue.LatitudeStaraZagora;
                    longitude = DefaultValue.LongitudeStaraZagora;
                    break;
                }

                case (int)RegionEnum.Haskovo:
                {
                    latitude = DefaultValue.LatitudePazarzhik;
                    longitude = DefaultValue.LongitudePazarzhik;
                    break;
                }
            }

            double distance = CalculateDistance(latitude, longitude, DefaultValue.DefaultSettlementLatitude, DefaultValue.DefaultSettlementLongitude);
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

        public int CalculateReadingTime(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return 0;
            }

            //200 is the number of words an average person reads per minute
            //dividing the words count per 200 gives the minutes 
            //it will take to read the text
            return (text.Split(' ').Length) / 200;
        }
    }

}
