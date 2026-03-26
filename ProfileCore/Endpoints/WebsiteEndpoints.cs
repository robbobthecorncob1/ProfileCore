using ProfileCore.Models.website;
using ProfileCore.Services;

namespace ProfileCore.Endpoints;

/// <summary>
/// Encapsulates the routing logic for website-specific operations.
/// </summary>
public static class WebsiteEndpoints
{
    /// <summary>
    /// Registers website-related Minimal API endpoints into the application's request pipeline.
    /// </summary>
    /// <param name="app">The <see cref="IEndpointRouteBuilder"/> to attach the routes to.</param>
    public static void MapWebsiteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/website/contact", HandleContactSubmission);
    }

    /// <summary>
    /// Handles the HTTP POST request for contact form submissions.
    /// Performs basic validation and coordinates with the <see cref="IEmailService"/> for delivery.
    /// </summary>
    /// <param name="submission">The data transfer object containing the user's message and contact info.</param>
    /// <param name="emailService">The injected service responsible for dispatching emails.</param>
    /// <returns>An <see cref="IResult"/> representing an HTTP 200 (OK), 400 (Bad Request), or 500 (Internal Server Error).</returns>
    private static async Task<IResult> HandleContactSubmission(ContactSubmission submission, IEmailService emailService)
    {
        Console.WriteLine($"\n--- NEW CONTACT REQUEST RECEIVED ---");
        Console.WriteLine($"From: {submission.Email}");
        
        try
        {
            if (string.IsNullOrWhiteSpace(submission.Email) || string.IsNullOrWhiteSpace(submission.Message)) 
            {
                Console.WriteLine("Validation failed: Missing Email or Message.");
                return Results.BadRequest("Email and Message are required.");
            }

            Console.WriteLine("Validation passed. Handing off to EmailService...");
            await emailService.SendContactEmailAsync(submission);
            
            Console.WriteLine("EmailService finished successfully!");
            return Results.Ok(new { message = "Email sent successfully!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CRITICAL EMAIL ERROR: {ex.Message}");
            return Results.StatusCode(500);
        }
    }
}