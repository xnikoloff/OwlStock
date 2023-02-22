using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.DTOs
{
    public class PhotoByIdDTO
    {
        public Photo? Photo { get; set; }
        public PhotoSize PhotoSize { get; set; }
    }
}
