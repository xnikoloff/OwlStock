namespace OwlStock.Domain.Common
{
    public static class ModelConstraints
    {
        //Photo
        public const int PictureNameMaxLength = 500;
        public const int PictureDescriptionMaxLength = 2000;
        public const int PictureGeoLocationMaxLength = 100;
        public const int PictureGearMaxLength = 1000;
        public const string PhotoFormFileDisplayName = "File";

        //PhotoShoot
        public const int PersonNameMaxLength = 50;
        public const int PersonEmailMaxLength = 200;
        public const int PersonPhoneMaxLenth = 30;
        public const int PhotoShootTypeDescriptionMaxLength = 500;


        //Common
        public const string TagsRequiredErrorMessage = "Tags are required";
        public const string CategoriesRequiredErrorMessage = "Categories are required";
        public const string FormFilesRequiredErrorMessage = "Upload a file";

        //City
        internal const int SettlementNameMaxLength = 100;
        internal const int PostCodeMaxLength = 10;
        internal const int LatLongMaxLength = 25;
    }
}
