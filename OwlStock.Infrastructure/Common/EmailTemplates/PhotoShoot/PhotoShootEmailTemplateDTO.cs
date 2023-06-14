using OwlStock.Domain.Enumerations;

namespace OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot
{
    public class PhotoShootEmailTemplateDTO : EmailTemplateBaseDTO
    {
        public DateTime Date { get; set; }
        public PhotoShootType Type { get; set; }
        public string? PersonFullName { get; set; }
    }
}
