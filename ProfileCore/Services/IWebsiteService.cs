using ProfileCore.Models;

namespace ProfileCore.Services;

/// <summary>
/// Defines the core business logic contract for retrieving portfolio data 
/// and processing user interactions.
/// </summary>
public interface IWebsiteService
{
    /// <summary>
    /// Asynchronously Retrieves the user's core profile information, biography, and linked social media accounts.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="Profile"/> record.</returns>
    Task<Profile> GetProfileAsync();
    
    /// <summary>
    /// Asynchronously Retrieves the user's complete employment history.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the  <see cref="WorkExperience"/> record.</returns>
     Task<WorkExperience> GetWorkExperienceAsync();
    
    /// <summary>
    /// Asynchronously Retrieves the user's portfolio of developed software projects and repositories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the  <see cref="ProjectExperience"/> record.</returns>
     Task<ProjectExperience> GetProjectExperienceAsync();
    
    /// <summary>
    /// Asynchronously Retrieves the user's academic history, degrees, and certifications.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the  <see cref="EducationList"/> record.</returns>
     Task<EducationList> GetEducationAsync();
    
    /// <summary>
    /// Asynchronously Retrieves the current operational health, environment, and version metrics of the application.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the  <see cref="SystemStatus"/> record.</returns>
     Task<SystemStatus> GetSystemStatusAsync();
    
    /// <summary>
    /// Asynchronously Processes a new contact form submission from a site visitor.
    /// </summary>
    /// <param name="submission">The payload containing the sender's details and message.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ProcessContactSubmissionAsync(ContactSubmission submission);
}