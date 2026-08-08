using Microsoft.Extensions.Configuration;
using OwlStock.Domain.Enumerations;
using System.Net;
using System.Net.Mail;
using OwlStock.Infrastructure.Common.EmailTemplates.PhotoShoot;
using OwlStock.Infrastructure.Common.EmailTemplates;
using OwlStock.Infrastructure.Common.EmailTemplates.Account;
using OwlStock.Infrastructure.Common.EmailTemplates.Inquiry;
using Microsoft.Extensions.Logging;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        private readonly string _smtpEmail;
        private readonly string _smtpDisplayName;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpKey;
        
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _smtpEmail = _configuration.GetValue<string>("Smpt:Email") ?? throw new NullReferenceException("[Email] email address is null");
            _smtpDisplayName = _configuration.GetValue<string>("Smpt:DisplayName") ?? throw new NullReferenceException("[DisplayName] email address is null");
            _smtpHost = _configuration.GetValue<string>("Smpt:Host") ?? throw new NullReferenceException("Host is null");
            _smtpPort = _configuration.GetValue<int>("Smpt:Port");
            _smtpUser = _configuration.GetValue<string>("Smpt:Login") ?? throw new NullReferenceException("Smtp:Login is null");
            _smtpKey = _configuration.GetValue<string>("Smpt:Key") ?? throw new NullReferenceException("Smtp:Key is null");
            _logger = logger;
        }

        public async Task SendInquiry(SendInquiryEmailTemplateDTO dto)
        {
            await Send(dto);
        }

        public async Task<bool> Send(EmailTemplateBaseDTO dto)
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

            try
            {
                if (dto.EmailTemplate is EmailTemplate.CreatePhotoShoot)
                {
                    //if email template is for created photoshoot
                    //send template to user and to Photonic
                    MailMessage messageUser = new();
                    MailMessage messagePhotonic = new();

                    messageUser.From = new MailAddress(_smtpEmail,  _smtpDisplayName);
                    messageUser.To.Add(dto.Recipient ?? "");
                    messageUser.Subject = dto.Topic ?? "";
                    messageUser.Body = GetTemplate(dto);

                    //second email is always sent to Photonic

                    messagePhotonic.From = new MailAddress(_smtpEmail,  _smtpDisplayName);
                    messagePhotonic.To.Add(_smtpEmail);
                    messagePhotonic.Subject = dto.Topic ?? "";
                    messagePhotonic.Body = GetTemplatePhoton(dto);

                    messages = new MailMessage[] { messageUser, messagePhotonic };
                }

                else
                {
                    //if email template is not for created photoshoot
                    //send template to user only

                    MailMessage messageUser = new();
                    messageUser.From = new MailAddress(_smtpEmail,  _smtpDisplayName);
                    messageUser.To.Add(dto.Recipient ?? "");
                    messageUser.Subject = dto.Topic ?? "";
                    messageUser.Body = GetTemplate(dto);

                    messages = new MailMessage[] { messageUser };
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return false;
            }

            for (int i = 0; i < messages.Length; i++) 
            {
                try
                {
                    messages[i].IsBodyHtml = true;
                    await client.SendMailAsync(messages[i]);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                    return false;
                }
            }

            return true;

        }

        public string GetTemplate(EmailTemplateBaseDTO dto)
        {
            switch (dto.EmailTemplate)
            {
                case EmailTemplate.CreatePhotoShoot:
                {
                    return PhotoShootEmailTemplates.CreatePhotoShootTemplate
                    (
                        ((PhotoShootEmailTemplateDTO)dto).Date ?? new DateTime(),
                        ((PhotoShootEmailTemplateDTO)dto).Type,
                        ((PhotoShootEmailTemplateDTO)dto).Price,
                        ((PhotoShootEmailTemplateDTO)dto).PhotoShootId
                    );
                }

                case EmailTemplate.UpdatePhotosForPhotoShoot:
                {
                    return PhotoShootEmailTemplates.UpdatePhotoShootTemplate
                        (
                            ((UpdatePhotoShootEmailTemplateDTO)dto).PhotoShootId
                        );
                }
                
                case EmailTemplate.UpdatePhotoShoot:
                {
                    return PhotoShootEmailTemplates.UpdatePhotoShootDataTemplate
                        (
                            ((UpdatePhotoshootDataEmailTemplateDTO)dto).PhotoShootId,
                            ((UpdatePhotoshootDataEmailTemplateDTO)dto).PhotoShootType,
                            ((UpdatePhotoshootDataEmailTemplateDTO)dto).PhotoshootNumber ?? "-",
                            ((UpdatePhotoshootDataEmailTemplateDTO)dto).ReservationDate,
                            ((UpdatePhotoshootDataEmailTemplateDTO)dto).Price
                        );
                }

                case EmailTemplate.DeclinePhotoShoot:
                {
                    return PhotoShootEmailTemplates.DeclinePhotoShootTemplate
                        (
                            ((UpdatePhotoShootEmailTemplateDTO)dto).PhotoShootId
                        );
                }

                case EmailTemplate.CancelPhotoShoot:
                {
                    return PhotoShootEmailTemplates.CancelPhotoShootTemplate
                    (
                        ((UpdatePhotoShootEmailTemplateDTO)dto).PhotoShootId
                    );
                }

                case EmailTemplate.CreateAccount:
                {
                    return AccountEmailTemplates.CreateAccountTemplate
                    (
                        ((CreateAccountEmailTemplateDTO)dto)?.Password ?? ""
                    );
                }

                case EmailTemplate.CreateConfirmedAccount:
                {
                    return AccountEmailTemplates.CreateConfirmedAccountTemplate();
                }

                case EmailTemplate.ConfirmAccount:
                {
                    return AccountEmailTemplates.ConfirmAccountTemplate
                    (
                        ((ConfirmAccountEmailTemplate)dto)?.ConfirmationLink ?? ""
                    );
                }

                case EmailTemplate.ResetPassword:
                {
                    return AccountEmailTemplates.ResetPasswordTemplate
                    (
                        ((ResetPasswordEmailTemplateDTO)dto)?.CallBackURL ?? ""
                    );
                }

                case EmailTemplate.SendInquiry:
                {
                    return InquiryEmailTemplates.SendInquiryTemplate
                    (
                        ((SendInquiryEmailTemplateDTO)dto)?.Name ?? "",
                        ((SendInquiryEmailTemplateDTO)dto)?.Content ?? ""
                    );
                }

                default:
                {
                    throw new ArgumentException($"{dto.EmailTemplate} is invalid {nameof(EmailTemplate)}");
                }
            }
        }

        public string GetTemplatePhoton(EmailTemplateBaseDTO dto)
        {
            switch (dto.EmailTemplate)
            {
                case EmailTemplate.CreatePhotoShoot:
                {
                        return PhotoShootEmailTemplates.CreatePhotoShootTemplateDreampix
                    (
                         ((PhotoShootEmailTemplateDTO)dto).Date ?? new DateTime(),
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
