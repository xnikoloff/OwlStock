using OwlStock.Domain.Enumerations;

namespace OwlStock.Infrastructure.Common.EmailTemplates
{
    public class EmailTemplateBaseDTO
    {
        public string? Recipient { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
    }
}
