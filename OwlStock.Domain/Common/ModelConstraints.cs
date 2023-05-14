namespace OwlStock.Domain.Common
{
    public static class ModelConstraints
    {
        //Photo
        public const int PictureNameMaxLength = 500;
        public const int PictureDescriptionMaxLength = 2000;
        public const string PhotoFormFileDisplayName = "File";

        //PhotoShoot
        public const int PersonNameMaxLength = 50;
        public const int PersonEmailMaxLength = 200;
        public const int PersonPhoneMaxLenth = 30;
        public const int PhotoShootTypeDescriptionMaxLength = 500;
    }
}
