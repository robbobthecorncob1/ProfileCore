using Resend;
using ProfileCore.Models;

namespace ProfileCore.Services;

/// <inheritdoc />
/// <param name="resend">The Resend client used for API communication.</param>
/// <param name="config">Application configuration for retrieving API keys and destination addresses.</param>
public class EmailService(IResend resend, IConfiguration config) : IEmailService
{
    private readonly IResend _resend = resend;
    private readonly IConfiguration _config = config;

    /// <inheritdoc />
    public async Task SendContactEmailAsync(ContactSubmission submission)
    {
        var toEmail = _config["Resend:ToEmail"]!;

        var message = new EmailMessage
        {
            From = "onboarding@resend.dev", 
            To = { toEmail },
            Subject = $"Portfolio Contact: {submission.Subject}",
            HtmlBody = $@"
                <h3>New message from your portfolio website!</h3>
                <p><strong>Name:</strong> {submission.Name}</p>
                <p><strong>Email:</strong> {submission.Email}</p>
                <hr />
                <p>{submission.Message.Replace("\n", "<br/>")}</p>"
        };

        Console.WriteLine("[Resend] Bypassing ISP firewalls. Sending via HTTP API...");
        await _resend.EmailSendAsync(message);
        Console.WriteLine("[Resend] Message sent successfully!");
    }
}