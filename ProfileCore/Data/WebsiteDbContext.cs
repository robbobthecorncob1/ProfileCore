using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProfileCore.Models.website;

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

    /// <summary>
    /// Gets or sets the database table containing the user's academic courses.
    /// </summary>
    public DbSet<Course> Courses {get; set; }

    /// <summary>
    /// Configures the database schema and entity mappings for the application.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Job>().Property(job => job.Technologies).HasConversion(
            list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
            jsonString => JsonSerializer.Deserialize<List<string>>(jsonString, (JsonSerializerOptions?)null) ?? new List<string>()
        );
    }
}