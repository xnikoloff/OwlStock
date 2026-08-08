using OwlStock.Domain.Entities;

namespace OwlStock.Services.DTOs.HomePage
{
    public class HomePageDTO
    {
        public string? Photo { get; set; }
        public IEnumerable<Article>? Articles { get; set; }
        public IEnumerable<Testimony>? Testimonies { get; set; }
    }
}
