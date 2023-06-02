using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure.Common.EmailTemplates;

namespace OwlStock.Services.Interfaces
{
    public interface IEmailService
    {
        Task Send(EmailTemplateBaseDTO dto);
    }
}
