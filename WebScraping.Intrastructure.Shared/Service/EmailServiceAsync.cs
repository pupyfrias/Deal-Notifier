using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using WebScraping.Core.Application.Constants;
using WebScraping.Core.Application.DTOs.Email;
using WebScraping.Infrastructure.Email.Settings;

namespace WebScraping.Infrastructure.Email.Service
{
    public class EmailServiceAsync : IEmailServiceAsync
    {
        private readonly MailSettings _mailSetttings;

        public EmailServiceAsync(IOptions<MailSettings> option)
        {
            _mailSetttings = option.Value;
        }

        public async Task SendAsync(EmailDto emailDto)
        {
            BodyBuilder body = new() { HtmlBody = emailDto.Body };
            MimeMessage email = new()
            {
                Subject = emailDto.Subject,
                Body = body.ToMessageBody()
            };

            email.To.Add(MailboxAddress.Parse(emailDto.To));
            email.From.Add(new MailboxAddress("Offer", _mailSetttings.EmailFrom));

            using (SmtpClient smtp = new())
            {
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_mailSetttings.SmtpHost, _mailSetttings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSetttings.SmtpUser, _mailSetttings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }
    }
}