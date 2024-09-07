using OwlStock.Domain.Entities;

namespace OwlStock.Web.DTOs.PlaceDTOs
{
    public class CreatePlaceDTO
    {
        public Place? Place { get; set; }
        public List<City>? Cities { get; set; }
        public IFormFile? File { get; set; }
        public string? RootPath { get; set; }
    }
}
