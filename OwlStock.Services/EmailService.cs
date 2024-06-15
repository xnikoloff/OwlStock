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
        private readonly IConfiguration _configuration;

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
            

            SmtpClient client = new(_smtpHost)
            {
                Port = _smtpPort,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_smtpUser, _smtpKey),
                EnableSsl = true,
            };

            MailMessage[] messages;

            if (dto.EmailTemplate is EmailTemplate.CreatePhotoShoot)
            {
                //if email template is for created photoshoot
                //send template to user and dreampix
                messages = new MailMessage[]
                {
                    new
                    (
                        "hristiyan.at.nikoloff@gmail.com",
                        dto.Recipient ?? throw new NullReferenceException($"{nameof(dto.Recipient)} is null"),
                        dto.Topic,
                        GetTemplate(dto)
                    ),

                    //second email is always sent to DreamPix
                    new
                    (
                        "hristiyan.at.nikoloff@gmail.com",
                        "hristiyan.at.nikoloff@gmail.com",
                        dto.Topic,
                        GetTemplateDreampix(dto)
                    ),

                };
            }

            else
            {
                //if email template is not for created photoshoot
                //send template to user only
                messages = new MailMessage[]
                {
                    new
                    (
                        "hristiyan.at.nikoloff@gmail.com",
                        dto.Recipient ?? throw new NullReferenceException($"{nameof(dto.Recipient)} is null"),
                        dto.Topic,
                        GetTemplate(dto)
                    )

                };
            }
            
            for(int i = 0; i < messages.Length; i++) 
            {
                await client.SendMailAsync(messages[i]);
            }

        }

        public string GetTemplate(EmailTemplateBaseDTO dto)
        {
            switch (dto.EmailTemplate)
            {
                case EmailTemplate.CreatePhotoShoot:
                {
                        return PhotoShootEmailTemplates.CreatePhotoShootTemplate(dto.PhotoShootId);
                }

                case EmailTemplate.UpdatePhotosForPhotoShoot:
                    {
                        return PhotoShootEmailTemplates.UpdatePhotoShootTemplate
                            (
                                ((UpdatePhotoShootEmailTemplateDTO)dto).PhotoShootId
                            );
                    }

                default:
                {
                    throw new ArgumentException($"{dto.EmailTemplate} is invalid {nameof(EmailTemplate)}");
                }
            }
        }

        public string GetTemplateDreampix(EmailTemplateBaseDTO dto)
        {
            switch (dto.EmailTemplate)
            {
                case EmailTemplate.CreatePhotoShoot:
                {
                        return PhotoShootEmailTemplates.CreatePhotoShootTemplateDreampix
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
