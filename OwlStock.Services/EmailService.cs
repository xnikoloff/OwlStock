using Microsoft.Extensions.Configuration;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot;
using OwlStock.Infrastructure.Common.EmailTemplates;

namespace OwlStock.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;

        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpKey;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpHost = _configuration.GetValue<string>("Smpt:Host") ?? throw new NullReferenceException("Host is null");
            _smtpPort = _configuration.GetValue<int>("Smpt:Port");
            _smtpUser = _configuration.GetValue<string>("Smpt:Login") ?? throw new NullReferenceException("Smtp:Login is null");
            _smtpKey = _configuration.GetValue<string>("Smpt:Key") ?? throw new NullReferenceException("Smtp:Key is null");
        }

        public async Task Send(EmailTemplateBaseDTO dto)
        {
            

            SmtpClient client = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_smtpUser, _smtpKey),
                EnableSsl = true,
            };

            MailMessage message =  new
            (
                "hristiyan.at.nikoloff@gmail.com",
                dto.Recipient ?? throw new NullReferenceException($"{nameof(dto.Recipient)} is null"), 
                "This is a test", 
                GetTemplate(dto)
            );
            
            await client.SendMailAsync(message);

        }

        public string GetTemplate(EmailTemplateBaseDTO dto)
        {
            switch (dto.EmailTemplate)
            {
                case EmailTemplate.CreatePhotoShoot:
                {

                     return PhotoShootEmailTemplates.CreatePhotoShootTemplate
                    (
                         ((PhotoShootEmailTemplateDTO)dto).PersonFullName ?? "",
                         ((PhotoShootEmailTemplateDTO)dto).Date,
                         ((PhotoShootEmailTemplateDTO)dto).Type
                    );
                }

                default:
                {
                    throw new ArgumentException($"{dto.EmailTemplate} is invalid {nameof(EmailTemplate)}");
                }
            }

            
        }
    }
}
