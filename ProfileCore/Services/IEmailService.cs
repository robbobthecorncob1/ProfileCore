using ProfileCore.Models.website;

namespace ProfileCore.Services;

/// <summary>
/// Provides email communication capabilities using the Resend HTTP API.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Formats and sends a contact form submission to the configured administrator email address.
    /// Converts plain text line breaks to HTML for consistent rendering in email clients.
    /// </summary>
    /// <param name="submission">The data model containing the sender's name, email, subject, and message body.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    Task SendContactEmailAsync(ContactSubmission submission);
}