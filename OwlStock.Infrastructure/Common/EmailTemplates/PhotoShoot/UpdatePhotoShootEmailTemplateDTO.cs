namespace OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot
{
    public class UpdatePhotoShootEmailTemplateDTO : EmailTemplateBaseDTO
    {
        public string? PersonFullName { get; set; }
        public string? Url { get; set; }
    }
}
