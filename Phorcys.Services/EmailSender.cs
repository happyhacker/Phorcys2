using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using PostmarkDotNet;

namespace Phorcys.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _postmarkApiKey;   // Postmark calls this a "Server Token"
        private readonly string? _messageStream;   // optional, e.g. "outbound" (default) or your custom stream
        private readonly string _fromAddress;

        public EmailSender(IConfiguration configuration)
        {
            // These map from Key Vault secrets:
            //   postmark--apikey          => Postmark:ApiKey
            //   postmark--messagestream   => Postmark:MessageStream   (optional)
            //   postmark--from            => Postmark:From            (optional convenience)
            _postmarkApiKey = configuration["Postmark:ApiKey"]
                              ?? throw new System.Exception("Missing configuration: Postmark:ApiKey");
            _messageStream = configuration["Postmark:MessageStream"]; // optional
            _fromAddress = configuration["Postmark:From"] ?? "Larry@Hacksoft.net"; // must be verified in Postmark
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new PostmarkClient(_postmarkApiKey);

            var pm = new PostmarkMessage
            {
                From = _fromAddress,   // Sender Signature or verified domain
                To = email,
                Subject = subject,
                HtmlBody = message,
                TextBody = StripHtml(message),   // recommended to include a text version
                TrackOpens = true,
                MessageStream = _messageStream       // omit if you don’t use custom streams
            };

            var resp = await client.SendMessageAsync(pm);
            if (resp.Status != PostmarkStatus.Success)
            {
                throw new System.Exception($"Postmark failed ({resp.ErrorCode}): {resp.Message}");
            }
        }

        private static string StripHtml(string html) =>
            System.Text.RegularExpressions.Regex.Replace(html ?? string.Empty, "<.*?>", string.Empty);
    }
}
