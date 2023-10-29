using Catalog.Application.Interfaces.Services;
using Catalog.Application.ViewModels.V1.Email;
using Email.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ILogger = Serilog.ILogger;

namespace Email.Service
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger _logger;

        public EmailService(IOptions<MailSettings> option, ILogger logger)
        {
            _mailSettings = option.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailDto emailDto)
        {

            try
            {
                BodyBuilder body = new() { HtmlBody = emailDto.Body };
                MimeMessage email = new()
                {
                    Subject = emailDto.Subject,
                    Body = body.ToMessageBody()
                };

                email.To.Add(MailboxAddress.Parse(emailDto.To));
                email.From.Add(new MailboxAddress("Offer", _mailSettings.EmailFrom));

                using (SmtpClient smtp = new())
                {
                    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                    await smtp.SendAsync(email);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while send Email. Error: {ex.Message}");
            }

        }

    }
}