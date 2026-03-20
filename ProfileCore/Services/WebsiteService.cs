using Microsoft.EntityFrameworkCore;
using ProfileCore.Data;
using ProfileCore.Models;

namespace ProfileCore.Services;

/// <inheritdoc />
/// <param name="context">The database context for website data.</param>
/// <param name="logger">The logger for service-level events.</param>
public class WebsiteService(WebsiteDbContext context, ILogger<WebsiteService> logger) : IWebsiteService
{
    private readonly WebsiteDbContext _context = context;
    private readonly ILogger<WebsiteService> _logger = logger;
    private static readonly DateTime _serverStartTime = DateTime.UtcNow;
    
    /// <inheritdoc />
    public async Task<Profile> GetProfileAsync()
    {
        return await _context.Profile.Include(p => p.SocialLinks).FirstOrDefaultAsync() ?? throw new InvalidOperationException("Profile data has not been seeded into the database yet.");
    }
    
    /// <inheritdoc />
    public async Task<WorkExperience> GetWorkExperienceAsync()
    {
        return new WorkExperience(await _context.Jobs.ToListAsync());
    }
    
    /// <inheritdoc />
    public async Task<ProjectExperience> GetProjectExperienceAsync()
    {
        return new ProjectExperience(await _context.Projects.ToListAsync());
    }
    
    /// <inheritdoc />
    public async Task<EducationList> GetEducationAsync()
    {
        return new EducationList(await _context.Education.ToListAsync());
    }
    
    /// <inheritdoc />
    public async Task<SystemStatus> GetSystemStatusAsync()
    {
        return await Task.FromResult(
            new SystemStatus(
                Environment: Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                Version: "1.0.0",
                ServerTime: DateTime.UtcNow,
                UptimeMilliseconds: (long)(DateTime.UtcNow - _serverStartTime).TotalMilliseconds,
                StatusMessage: "API and Database are online and healthy."
            )
        );
    }
    
    /// <inheritdoc />
    public async Task ProcessContactSubmissionAsync(ContactSubmission submission)
    {
        _logger.LogInformation(
            "New Contact Form Submission Received!\nName: {Name}\nEmail: {Email}\nSubject: {Subject}\nMessage: {Message}",
            submission.Name, submission.Email, submission.Subject, submission.Message
        );
        await Task.CompletedTask;
    }
}