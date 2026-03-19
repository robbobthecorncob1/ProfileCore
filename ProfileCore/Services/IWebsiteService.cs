using ProfileCore.Models;

namespace ProfileCore.Services;

/// <summary>
/// Defines the core business logic contract for retrieving portfolio data 
/// and processing user interactions.
/// </summary>
public interface IWebsiteService
{
    /// <summary>
    /// Retrieves the user's core profile information, biography, and linked social media accounts.
    /// </summary>
    /// <returns>A complete <see cref="Profile"/> record.</returns>
    Profile GetProfile();
    
    /// <summary>
    /// Retrieves the user's complete employment history.
    /// </summary>
    /// <returns>A <see cref="WorkExperience"/> record containing a list of past and present jobs.</returns>
    WorkExperience GetWorkExperience();
    
    /// <summary>
    /// Retrieves the user's portfolio of developed software projects and repositories.
    /// </summary>
    /// <returns>A <see cref="ProjectExperience"/> record containing a list of projects.</returns>
    ProjectExperience GetProjectExperience();
    
    /// <summary>
    /// Retrieves the user's academic history, degrees, and certifications.
    /// </summary>
    /// <returns>An <see cref="EducationList"/> record containing a list of educational programs.</returns>
    EducationList GetEducation();
    
    /// <summary>
    /// Retrieves the current operational health, environment, and version metrics of the application.
    /// </summary>
    /// <returns>A <see cref="SystemStatus"/> record detailing system health.</returns>
    SystemStatus GetSystemStatus();
    
    /// <summary>
    /// Processes a new contact form submission from a site visitor, typically via email or internal logging.
    /// </summary>
    /// <param name="submission">The payload containing the sender's details and message.</param>
    void ProcessContactSubmission(ContactSubmission submission);
}