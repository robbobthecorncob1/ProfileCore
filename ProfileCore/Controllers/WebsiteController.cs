using Microsoft.AspNetCore.Mvc;
using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Controllers;

/// <summary>
/// The primary API controller for the portfolio website.
/// Handles incoming HTTP requests from the frontend and routes them to the underlying service layer.
/// </summary>
/// <param name="websiteService">The injected business logic service that processes portfolio data.</param>
[ApiController]
[Route("api/[controller]")]
public class WebsiteController(IWebsiteService websiteService) : ControllerBase
{
    private readonly IWebsiteService _websiteService = websiteService;

    /// <summary>
    /// Retrieves the user's primary profile information, biography, and skills.
    /// </summary>
    /// <returns>The complete profile record.</returns>
    /// <response code="200">Returns the requested profile data.</response>
    [HttpGet("profile")]
    public async Task<ActionResult<Profile>> GetProfile()
    {
        return Ok(await _websiteService.GetProfileAsync());
    }

    /// <summary>
    /// Retrieves the user's complete employment history.
    /// </summary>
    /// <returns>A list of past and present jobs.</returns>
    /// <response code="200">Returns the requested work experience data.</response>
    [HttpGet("experience")]
    public async Task <ActionResult<List<Job>>> GetWorkExperience()
    {
        return Ok(await _websiteService.GetWorkExperienceAsync());
    }

    /// <summary>
    /// Retrieves the user's portfolio of developed software projects.
    /// </summary>
    /// <returns>A list of projects.</returns>
    /// <response code="200">Returns the requested project data.</response>
    [HttpGet("projects")]
    public async Task<ActionResult<List<Project>>> GetProjects()
    {
        return Ok(await _websiteService.GetProjectsAsync());
    }

    /// <summary>
    /// Retrieves the user's portfolio of skills from their projects and jobs.
    /// </summary>
    /// <returns>A list of skills.</returns>
    /// <response code="200">Returns the requested skills data.</response>
    [HttpGet("skills")]
    public async Task<ActionResult<List<Skill>>> GetSkills()
    {
        return Ok(await _websiteService.GetSkillsAsync());
    }

    /// <summary>
    /// Retrieves the user's academic history, degrees, and certifications.
    /// </summary>
    /// <returns>A list of educational programs.</returns>
    /// <response code="200">Returns the requested education data.</response>
    [HttpGet("education")]
    public async Task<ActionResult<List<EducationProgram>>> GetEducation()
    {
        return Ok(await _websiteService.GetEducationAsync());
    }

    /// <summary>
    /// Retrieves the current operational health, environment, and version of the API.
    /// </summary>
    /// <returns>A system status record.</returns>
    /// <response code="200">Returns the current system health metrics.</response>
    [HttpGet("status")]
    public async Task<ActionResult<SystemStatus>> GetSystemStatus()
    {
        return Ok(await _websiteService.GetSystemStatusAsync());
    }

}