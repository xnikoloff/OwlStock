using Microsoft.Extensions.Configuration;
using OwlStock.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace OwlStock.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send()
        {
            string smptHost = _configuration.GetValue<string>("Smpt:Host") ?? throw new NullReferenceException("Host is null");
            int smptPort = _configuration.GetValue<int>("Smpt:Port");
            string smptUser = _configuration.GetValue<string>("Smpt:Login") ?? throw new NullReferenceException("Smtp:Login is null");
            string smptKey = _configuration.GetValue<string>("Smpt:Key") ?? throw new NullReferenceException("Smtp:Key is null");

            SmtpClient client = new SmtpClient(smptHost)
            {
                Port = smptPort,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(smptUser, smptKey),
                EnableSsl = true,
            };

            MailMessage message =  new MailMessage
            (
                "hristiyan.at.nikoloff@gmail.com", 
                "hristiyan.at.nikolov@icloud.com", 
                "This is a test", 
                "Test email"
            );
            
            await client.SendMailAsync(message);

        }
    }
}
