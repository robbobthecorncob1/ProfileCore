using Microsoft.EntityFrameworkCore;
using ProfileCore.Models;

namespace ProfileCore.Data;

/// <summary>
/// The central database context for the portfolio website. 
/// Acts as the bridge between the C# application and the underlying SQLite database.
/// </summary>
/// <param name="options">The configuration options used to connect to the database.</param>
public class WebsiteDbContext(DbContextOptions<WebsiteDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the database table containing the user's primary profile, biography, and skills.
    /// </summary>
    public DbSet<Profile> Profile { get; set; }

    /// <summary>
    /// Gets or sets the database table containing the user's employment history and professional roles.
    /// </summary>
    public DbSet<Job> Jobs { get; set; }

    /// <summary>
    /// Gets or sets the database table containing the user's portfolio of developed software projects and repositories.
    /// </summary>
    public DbSet<Project> Projects { get; set; }

    /// <summary>
    /// Gets or sets the database table containing the user's academic history, degrees, and certifications.
    /// </summary>
    public DbSet<EducationProgram> Education { get; set; }
}