using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProfileCore.Data;
using ProfileCore.Models.website;

namespace ProfileCore.Services;

/// <inheritdoc />
/// <param name="context">The database context for website data.</param>
/// <param name="logger">The logger for service-level events.</param>
public class WebsiteService(WebsiteDbContext context, ILogger<WebsiteService> logger, IHostEnvironment env, WebsiteDbContext dbContext) : IWebsiteService
{
    private readonly WebsiteDbContext _context = context;
    private readonly ILogger<WebsiteService> _logger = logger;
    private readonly IHostEnvironment _env = env;
    private readonly WebsiteDbContext _dbContext = dbContext;
    private static readonly DateTime _serverStartTime = DateTime.UtcNow;
    
    /// <inheritdoc />
    public async Task<Profile> GetProfileAsync()
    {
        return await _context.Profile.Include(p => p.SocialLinks).FirstOrDefaultAsync() ?? throw new InvalidOperationException("Profile data has not been seeded into the database yet.");
    }
    
    /// <inheritdoc />
    public async Task<List<Job>> GetWorkExperienceAsync()
    {
        return await _context.Jobs.ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<List<Project>> GetProjectsAsync()
    {
        return await _context.Projects.ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<List<Skill>> GetSkillsAsync()
{
    var jobs = await GetWorkExperienceAsync();
    var projects = await GetProjectsAsync();

    var skillDictionary = new Dictionary<string, Skill>(StringComparer.OrdinalIgnoreCase);

    foreach (var project in projects)
    {
        if (project.Technologies != null)
        {
            foreach (var skillName in project.Technologies)
            {
                if (!skillDictionary.ContainsKey(skillName)) skillDictionary[skillName] = new Skill(skillName);

                skillDictionary[skillName].ProjectsSkillUsed.Add(project);
            }
        }
    }

    foreach (var job in jobs)
    {
        if (job.Technologies != null)
        {
            foreach (var skillName in job.Technologies)
            {
                if (skillName.Length > 0) {
                    if (!skillDictionary.ContainsKey(skillName)) skillDictionary[skillName] = new Skill(skillName);

                    skillDictionary[skillName].JobsSkillUsed.Add(job);
                }
            }
        }
    }

    return [.. skillDictionary.Values];
}

    /// <inheritdoc />
    public async Task<List<EducationProgram>> GetEducationAsync()
    {
        return await _context.Education.ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<SystemStatus> GetSystemStatusAsync()
    {
        string dbStatus = "Offline";
        try 
        {
            bool isDbOnline = await _dbContext.Database.CanConnectAsync();
            if (isDbOnline) dbStatus = "Operational";
        }
        catch (Exception ex)
        {
            dbStatus = "Offline"; 
            Console.WriteLine($"DB Health Check Failed: {ex.Message}");
        }
        
        return await Task.FromResult(
            new SystemStatus(
                Environment: _env.EnvironmentName,
                Version: Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "Unknown",
                ServerTime: DateTime.UtcNow,
                UptimeMilliseconds: (long)(DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalMilliseconds,
                ApiStatus: "Operational",
                DatabaseStatus: dbStatus
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

    /// <inheritdoc />
    public async Task<List<Course>> GetCoursesAsync()
    {
        return await _context.Courses.ToListAsync();
    }
}