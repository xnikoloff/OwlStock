using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;

namespace OwlStock.Services.DTOs
{
    public class CreatePlacePhotoFileDTO
    {
        public Guid PlaceId { get; set; }

        public PhotoBase? PhotoBase { get; set; }

        public IFormFile? File { get; set; }

    }
}
