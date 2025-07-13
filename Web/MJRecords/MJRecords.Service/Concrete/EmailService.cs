using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email message asynchronously.
        /// </summary>
        /// <param name="message">The email message to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendAsync(EmailMessage message);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(SmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendAsync(EmailMessage message)
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl
            };

            var mailMessage = new MailMessage(_smtpSettings.From, message.To, message.Subject, message.Body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mailMessage);
        }
    }

    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public class SmtpSettings
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 25;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string From { get; set; } = "admin@tetorealstate.ca";
        public bool EnableSsl { get; set; } = true;
    }
}
