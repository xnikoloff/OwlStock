using Microsoft.IdentityModel.Tokens;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Common;
using OwlStock.Services.Common.Enumerations;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class CalculationsService : ICalculationsService
    {
        /// <summary>
        /// Calculates total of a photoshoot based on photoshoot type. If fuel price is set, it adds it to the total sum, 
        /// else returns the photoshoot price only. Price is multiplied by the number of participants when needed
        /// </summary>
        /// <param name="type">The type of the photoshoot</param>
        /// <param name="fuelPrice">THe fuel price</param>
        /// <param name="numberOfParticipants">Number of participants in the photoshoot</param>
        /// <returns>Total price of the photoshoot</returns>
        public decimal CalculatePhotoshootPrice(PhotoShootType type, decimal fuelPrice = 0, int numberOfParticipants = 1)
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

                case PhotoShootType.PersonalPlus:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.PortrairPlusPhotoShoot;
                    }

                    return DefaultValue.PortrairPlusPhotoShoot + fuelPrice;
                }

                case PhotoShootType.PersonalExtra:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.PortrairExtraPhotoShoot;
                    }

                    return DefaultValue.PortrairExtraPhotoShoot + fuelPrice;
                }

                case PhotoShootType.Family:
                    {
                        if (fuelPrice == 0)
                        {
                            return DefaultValue.FamilyPhotoShoot;
                        }

                        return DefaultValue.FamilyPhotoShoot + fuelPrice;
                    }

                case PhotoShootType.FamilyPlus:
                    {
                        if (fuelPrice == 0)
                        {
                            return DefaultValue.FamilyPlusPhotoShoot;
                        }

                        return DefaultValue.FamilyPlusPhotoShoot + fuelPrice;
                    }

                case PhotoShootType.FamilyExtra:
                    {
                        if (fuelPrice == 0)
                        {
                            return DefaultValue.FamilyExtraPhotoShoot;
                        }

                        return DefaultValue.FamilyExtraPhotoShoot + fuelPrice;
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

                case PhotoShootType.WeddingExtra:
                    {
                        if (fuelPrice == 0)
                        {
                            return DefaultValue.WeddingExtraPhotoshoot;
                        }

                        return DefaultValue.WeddingExtraPhotoshoot + fuelPrice;
                    }

                case PhotoShootType.Prom:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.PromPhotoshoot;
                    }

                    return DefaultValue.PromPhotoshoot + fuelPrice;
                }

                case PhotoShootType.PromPlus:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.PromPlusPhotoshoot;
                    }

                    return DefaultValue.PromPlusPhotoshoot + fuelPrice;
                }

                case PhotoShootType.PromExtra:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.PromExtraPhotoshoot;
                    }

                    return DefaultValue.PromExtraPhotoshoot + fuelPrice;
                }

                    case PhotoShootType.Baptism:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.Baptism;
                    }

                    return DefaultValue.Baptism + fuelPrice;
                }

                case PhotoShootType.BaptismPlus:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.BaptismPlus;
                    }

                    return DefaultValue.BaptismPlus + fuelPrice;
                }

                case PhotoShootType.BaptismExtra:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.BaptismExtra;
                    }

                    return DefaultValue.BaptismExtra + fuelPrice;
                }

                case PhotoShootType.BusinessPortrait:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.BusinessPortrait * numberOfParticipants;
                    }

                    return (DefaultValue.BusinessPortrait * numberOfParticipants) + fuelPrice;
                }

                case PhotoShootType.Automotive:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.Automotive;
                    }

                    return DefaultValue.Automotive + fuelPrice;
                }

                case PhotoShootType.AutomotivePlus:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.AutomotivePlus;
                    }

                    return DefaultValue.AutomotivePlus + fuelPrice;
                }

                case PhotoShootType.AutomotiveExtra:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.AutomotiveExtra;
                    }

                    return DefaultValue.AutomotiveExtra + fuelPrice;
                }

                case PhotoShootType.Product:
                {
                    if (fuelPrice == 0)
                    {
                        return DefaultValue.Product * numberOfParticipants;
                    }

                    return (DefaultValue.Product * numberOfParticipants) + fuelPrice;
                }

                case PhotoShootType.ProductPlus:
                    {
                        if (fuelPrice == 0)
                        {
                            return DefaultValue.ProductPlus * numberOfParticipants;
                        }

                        return (DefaultValue.ProductPlus * numberOfParticipants) + fuelPrice;
                    }

                default: return fuelPrice;
            }
        }

        /// <summary>
        /// Calculates the fuel price based on the set region.
        /// </summary>
        /// <param name="regionId">Id of the region</param>
        /// <returns>Price of the fuel</returns>
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

        /// <summary>
        /// Calculates the distance in KM between two points by latitude and longitude
        /// </summary>
        /// <param name="latitudeA">Latitude of the first point</param>
        /// <param name="longitudeA">Longitude of the first point</param>
        /// <param name="latitudeB">Latitude of the second point</param>
        /// <param name="longitudeB">Longitude of the second point</param>
        /// <returns>Distance in KM</returns>
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

        /// <summary>
        /// Calculates price of a trip by multiplying the fixed price by km to the distance that will be travelled. A fixed trip tax is added, too.
        /// </summary>
        /// <param name="distance">Distance that will be travelled</param>
        /// <returns>Price of the trip</returns>
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

        /// <summary>
        /// Calculation that time needed for text to be read in minutes
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>Time in minutes</returns>
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
