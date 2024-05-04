namespace OwlStock.Services.Common
{
    internal static class DefaultValue
    {
        //Photoshoots
        internal const decimal PortrairPhotoShoot = 150m;
        internal const decimal PregnantPhotoshoot = 100m;
        internal const decimal WeddingPhotoshoot = 200m;
        internal const decimal PromPhotoshoot = 400m;
        internal const decimal KidsPhotoshoot = 200m;
        internal const decimal Automotive = 100m;

        //Fuel
        internal const decimal FuelPriceByKilometer = 0.18m;
        internal const decimal TripTax = 6m;

        //Current location
        internal const string DefaultSettlement = "Асеновград";
        internal const double DefaultSettlementLatitude = 42.010493;
        internal const double DefaultSettlementLongitude = 24.878557;
    }
}
