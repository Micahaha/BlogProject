using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using PostmarkDotNet;

namespace BlogProject.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.PostMarkKey))
            {
                throw new Exception("Null PostMarkKey");
            }
            await ExecuteAsync(Options.PostMarkKey, subject, message, toEmail);
        }

        public async static Task ExecuteAsync(string apiKey, string subject, string message, string toEmail)
        {
            var client = new PostmarkClient(apiKey);
            var msg = new PostmarkMessage
            {
                From = "cahmiw@micahshouse.me",
                Subject = subject,
                TextBody = message,
                HtmlBody = message,
                To = toEmail
            };
            
            await client.SendMessageAsync(msg);

        }
    }

}
