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
        internal const decimal Baptism = 300m;

        //Fuel
        internal const decimal FuelPriceByKilometer = 0.18m;
        internal const decimal TripTax = 6m;

        //not used for now
        //Speed (in KM/h)
        //internal const int Speed = 60;
        
        //Current location
        internal const string DefaultSettlement = "Асеновград";
        internal const double DefaultSettlementLatitude = 42.010493;
        internal const double DefaultSettlementLongitude = 24.878557;

        //Serviced Regions GeoData
        public const double LatitudePlovdiv = 42.143543;
        public const double LongitudePlovdiv = 24.751459;
        public const double LatitudeHaskovo = 41.934594;
        public const double LongitudeHaskovo = 25.555545;
        public const double LatitudeStaraZagora = 42.425663;
        public const double LongitudeStaraZagora = 25.634676;
        public const double LatitudePazarzhik = 42.190478;
        public const double LongitudePazarzhik = 24.336608;

    }
}
