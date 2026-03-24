using System.ComponentModel.DataAnnotations;

namespace ProfileCore.Models;

/// <summary>
/// Represents the core profile information and biography of the portfolio owner.
/// </summary>
/// <param name="Id">The unique database identifier for the profile.</param>
/// <param name="FirstName">The owner's first name.</param>
/// <param name="LastName">The owner's last name.</param>
/// <param name="Headline">A short, professional headline.</param>
/// <param name="Bio">A detailed professional summary and background.</param>
/// <param name="ResumeUrl">An optional link to a downloadable resume document.</param>
/// <param name="Skills">A collection of professional skills or keywords.</param>
public record Profile (
    int Id,

    [property: Required, MaxLength(100)]
    string FirstName,

    [property: Required, MaxLength(100)]
    string LastName,

    [property: Required, MaxLength(2000)]
    string Headline,

    [property: Required, MaxLength(20000)]
    string Bio,

    [property: MaxLength(500)]
    string? ResumeUrl,

    List<string> Skills
) { /// <summary>
    /// A collection of related social media and professional links.
    /// Initialized outside the primary constructor to allow Entity Framework Core 
    /// to map the relationship after object instantiation.
    /// </summary>
    public List<SocialLink> SocialLinks { get; init; } = [];
}

/// <summary>
/// Represents a hyperlink to a social media platform or external portfolio.
/// </summary>
/// <param name="Id">The unique database identifier for the social link.</param>
/// <param name="Platform">The name of the platform.</param>
/// <param name="Url">The direct hyperlink to the user's profile on the platform.</param>
public record SocialLink (
    int Id,

    [property: Required, MaxLength(100)]
    string Platform,

    [property: Required, MaxLength(200)]
    string Url
);

/// <summary>
/// Represents a single employment position or role.
/// </summary>
/// <param name="Id">The unique database identifier for the job.</param>
/// <param name="IsCurrentJob">Indicates if the user is currently employed in this role.</param>
/// <param name="Position">The professional title held during this employment.</param>
/// <param name="Company">The name of the employer or company.</param>
/// <param name="Technologies">A list of tools, languages, or frameworks utilized in this role.</param>
/// <param name="DateStarted">The month and year employment began.</param>
/// <param name="DateEnded">The month and year employment ended (null or empty if current).</param>
/// <param name="Description">A detailed summary of responsibilities and achievements.</param>
public record Job (
    int Id,

    bool? IsCurrentJob,

    [property: Required, MaxLength(100)]
    string Position,

    [property: Required, MaxLength(100)]
    string Company,

    List<string> Technologies,

    [property: Required, MaxLength(100)]
    string DateStarted,

    [property: MaxLength(100)]
    string? DateEnded,

    [property: Required, MaxLength(20000)]
    string Description
);

/// <summary>
/// Represents a software application, tool, or project built by the user.
/// </summary>
/// <param name="Id">The unique database identifier for the project.</param>
/// <param name="IsComplete">Indicates if the project has been finished or is still in development.</param>
/// <param name="Name">The title of the project.</param>
/// <param name="Technologies">A list of tools, languages, or frameworks used to build the project.</param>
/// <param name="Description">A detailed summary of the project's purpose and features.</param>
/// <param name="RepoURL">An optional hyperlink to the project's source code repository.</param>
/// <param name="DateStarted">The month and year the project was initiated.</param>
/// <param name="DateEnded">The month and year the project was concluded.</param>
public record Project (
    int Id,

    bool? IsComplete,

    [property: Required, MaxLength(100)]
    string Name,

    List<string> Technologies,

    [property: Required, MaxLength(20000)]
    string Description,

    [property: MaxLength(100)]
    string? RepoURL,

    [property: MaxLength(100)]
    string? DateStarted,

    [property: MaxLength(100)]
    string? DateEnded
);

/// <summary>
/// Represents a single skill. Has variables holding all jobs and projects where the skill is used.
/// </summary>
/// <param name="name">The name of the skill or technology</param>
public class Skill(string name)
{
    public string SkillName {get; set;} = name;
    public List<Job> JobsSkillUsed {get; set;} = [];
    public List<Project> ProjectsSkillUsed {get; set;} = [];
}

/// <summary>
/// Represents an academic degree, certification, or program of study.
/// </summary>
/// <param name="Id">The unique database identifier for the education program.</param>
/// <param name="IsComplete">Indicates if the degree or certification has been fully awarded.</param>
/// <param name="Degree">The type of degree or certification.</param>
/// <param name="Major">The primary field of study.</param>
/// <param name="School">The name of the university or institution.</param>
/// <param name="College">The specific college within the institution.</param>
/// <param name="Gpa">The grade point average achieved, restricted between 0.0 and 5.0.</param>
/// <param name="DateStarted">The month and year the program began.</param>
/// <param name="DateEnded">The month and year the program concluded or is expected to conclude.</param>
/// <param name="Description">A summary of academic achievements, honors, or relevant coursework.</param>
public record EducationProgram (
    int Id,

    bool IsComplete,

    [property: Required, MaxLength(100)]
    string Degree,

    [property: MaxLength(100)]
    string? Major,

    [property: Required, MaxLength(100)]
    string School,

    [property: MaxLength(100)]
    string? College,

    [property: Range(0.000, 5.000)]
    double? Gpa,

    [property: MaxLength(100)]
    string? DateStarted,

    [property: MaxLength(100)]
    string? DateEnded,

    [property: MaxLength(20000)]
    string? Description
);

/// <summary>
/// Represents a message payload submitted by a user via the website's contact form.
/// Note: This is a Data Transfer Object (DTO) and is not stored in the database.
/// </summary>
/// <param name="Name">The name of the sender.</param>
/// <param name="Email">The sender's return email address.</param>
/// <param name="Subject">The topic of the message.</param>
/// <param name="Message">The body of the message.</param>
public record ContactSubmission (
    [MaxLength(100)] string Name,
    [MaxLength(100)] string Email,
    [MaxLength(100)] string Subject,
    [MaxLength(10000)] string Message
);

/// <summary>
/// Represents the current operational health, environment, and version of the API.
/// </summary>
/// <param name="Environment">The current hosting environment.</param>
/// <param name="Version">The current deployment version of the application.</param>
/// <param name="ServerTime">The current localized time of the host server.</param>
/// <param name="UptimeMilliseconds">The total time the application has been running since the last restart.</param>
/// <param name="StatusMessage">A brief string indicating the health state.</param>
public record SystemStatus (
    string Environment,
    string Version,
    DateTime ServerTime,
    long UptimeMilliseconds,
    string ApiStatus,
    string DatabaseStatus
);