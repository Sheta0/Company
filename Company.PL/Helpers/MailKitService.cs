
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.PL.Helpers
{
    public class MailKitService : IMailKitService
    {
        private readonly IOptions<MailKit> _options;

        public MailKitService(IOptions<MailKit> options)
        {
            _options = options;
        }

        public async Task<bool> SendEmailAsync(Email email)
        {
            try
            {
                var mail = new MimeMessage();

                mail.Subject = email.Subject;
                mail.From.Add(new MailboxAddress (_options.Value.DisplayName, _options.Value.Email));
                mail.To.Add(MailboxAddress.Parse(email.To));

                var builder = new BodyBuilder();
                builder.HtmlBody = email.Body;
                mail.Body = builder.ToMessageBody();

                // Establish Connection
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_options.Value.Host, _options.Value.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_options.Value.Email, _options.Value.Password);

                // Send Email
                await smtp.SendAsync(mail);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
