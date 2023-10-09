using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.ViewModels.V1.Email;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Email.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Concurrent;
using System.Text;
using ILogger = Serilog.ILogger;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Email.Service
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

        public async Task SendAsync(EmailDto emailDto)
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
                smtp.Disconnect(true);
            }
        }


        public async Task NotifyByEmail(ConcurrentBag<Item> items)
        {
            if (items.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();

                var orderItemToNotifyList = items.OrderBy(item => item.Price);
                _logger.Information($"orderItemToNotifyList {orderItemToNotifyList.Count()}");

                foreach (var item in orderItemToNotifyList)
                {
                    var probability = item.UnlockProbabilityId == 3 ? Enums.UnlockProbability.High.ToString() : Enums.UnlockProbability.Middle.ToString();

                    stringBuilder.AppendFormat(
                        @"<div style=""margin: 50px 20px;border-radius: 10px;box-shadow: 0px 0px 20px 0px rgba(0, 0, 0, 0.4);padding: 10px;"">
                        <h2 style='margin:0;'>{0}</h2>
                        <p style ='font-size:large; margin:0;'>US$ {1}</p>
                        <p style ='font-size:large; margin:0;'>Unlock Probability: {5}</p>
                        <a href='https://10.0.0.3:8081/api/Items/{4}/cancel-notification' style= 'display:block'> Cancel Notification</a>
                        <a href='{2}'>
                            <img src='{3}' style=""width: 540px;height: 720px;object-fit: cover;""/>
                        </a>
                    </div>", item.Name, item.Price, item.Link, item.Image, item.Id, probability);
                }
                string body = stringBuilder.ToString();

                var email = new EmailDto
                {
                    To = "pupyfrias@gmail.com",
                    Subject = "Phones Offer",
                    Body = body
                };

                await SendAsync(email);
                items.Clear();
            }
        }
    }
}