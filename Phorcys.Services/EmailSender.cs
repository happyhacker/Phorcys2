using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace Phorcys.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridApiKey;

        public EmailSender(IConfiguration configuration)
        {
            // Fetch the API key from configuration
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Initialize the SendGrid client with the API key
            var client = new SendGridClient(_sendGridApiKey);

            // Create the email message
            var from = new EmailAddress("Larry@Hacksoft.net", "Phorcys App"); // Replace with your verified SendGrid email
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: message);

            // Send the email
            var response = await client.SendEmailAsync(msg);

            // Check for errors
            if ((int)response.StatusCode >= 400)
            {
                throw new System.Exception($"SendGrid failed with status code {(int)response.StatusCode}: {await response.Body.ReadAsStringAsync()}");
            }
        }
    }
}


