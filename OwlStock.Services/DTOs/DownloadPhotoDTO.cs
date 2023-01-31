using OwlStock.Domain;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.DTOs
{
    public class DownloadPhotoDTO
    {
        public Photo? Photo { get; set; }
        public PhotoSize PhotoSize { get; set; }
    }
}
