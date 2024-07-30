using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using StockApp.Core.Application.Dtos.Email;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Domain.Settings;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Tls.Crypto;

namespace StoackApp.Infraestructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private MailSettings _mailSettings { get; }
        public EmailService(IOptions<MailSettings> _mailSettings)
        {
            this._mailSettings = _mailSettings.Value;
        }
        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                MimeMessage email = new();
                email.Sender= MailboxAddress.Parse($"{_mailSettings.DisplayName} <{_mailSettings.EmailFrom}>");
                //email.From.Add(MailboxAddress.Parse("podcastdehoy@gmail.com"));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                BodyBuilder builder = new();
                builder.HtmlBody = request.Body;
                email.Body=builder.ToMessageBody();


                using SmtpClient smtp = new SmtpClient();
                //this is ToString jump the protocols of security
                //smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);

                smtp.Authenticate(_mailSettings.SmtpUser,_mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (Exception ex) { 
            }
        }

    }
}
