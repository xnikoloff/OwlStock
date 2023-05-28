using OwlStock.Domain.Enumerations;

namespace OwlStock.Domain.Entities
{
    public class PhotoCategory
    {
        public Guid Id { get; set; }
        public Guid PhotoId { get; set; }
        public Photo? Photo { get; set; }
        public Category Category { get; set; }
    }
}
