using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.Interfaces
{
    public interface ICalculationsService
    {
        /// <summary>
        /// Calulates the total price of a photoshot by adding the cost of the fuel to the base photoshoot price
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fuelPrice"></param>
        /// <returns></returns>
        decimal CalculatePhotoshootPrice(PhotoShootType type, decimal fuelPrice);

        /// <summary>
        /// Calculates the fuel price based on the distance of the provided settlement data
        /// </summary>
        /// <param name="settlementData">0th element is the region, 1st element is the settlement</param>
        /// <returns></returns>
        Task<decimal> CalculateFuelPrice(string[] settlementData);

        /// <summary>
        /// Calculates price by provided latitude and longitude for two settlements
        /// </summary>
        /// <param name="latitudeA"></param>
        /// <param name="longitudeA"></param>
        /// <param name="latitudeB"></param>
        /// <param name="longitudeAB"></param>
        /// <returns></returns>
        double CalculateDistance(double latitudeA, double longitudeA, double latitudeB, double longitudeAB);

        /// <summary>
        /// Calculates fuel price by provided distance
        /// </summary>
        /// <param name="distance">Distance in KM</param>
        /// <returns></returns>
        decimal CalculatePriceByDistance(double distance);
    }
}
