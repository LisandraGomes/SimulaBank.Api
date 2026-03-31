using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace SimulaBank.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient(configuration["Smtp:Host"])
            {
                Port = int.Parse(configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(
                    configuration["Smtp:User"],
                    configuration["Smtp:Password"]
                ),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var @from = (_smtpClient.Credentials as NetworkCredential)?.UserName
                       ?? throw new InvalidOperationException("SMTP credentials não configuradas corretamente.");

                using var mailMessage = new MailMessage(from, to, subject, body);
                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

    }
}
