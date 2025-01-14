using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using Project.Models;
using MailKit.Security; // Add this for SecureSocketOptions

namespace Project.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    // Connect to the Gmail SMTP server on port 587 with STARTTLS
                    await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

                    // Authenticate using your email and password
                    await client.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);

                    // Send the email
                    await client.SendAsync(emailMessage);

                    // Disconnect after sending the email
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    // Log or rethrow the error
                    throw new Exception($"Error sending email: {ex.Message}");
                }
            }
        }
    }
}
