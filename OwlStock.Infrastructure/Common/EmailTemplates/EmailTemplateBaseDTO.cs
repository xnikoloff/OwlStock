using OwlStock.Domain.Enumerations;

namespace OwlStock.Infrastructure.Common.EmailTemplates
{
    public class EmailTemplateBaseDTO
    {
        public string? Recipient { get; set; }
        public string? Topic { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
        public Guid PhotoShootId { get; set; }
    }
}
