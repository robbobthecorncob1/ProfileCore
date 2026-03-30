using Microsoft.AspNetCore.Mvc;
using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Controllers;

/// <summary>
/// Handles all incoming HTTP web traffic related to Grade Point Average calculations.
/// </summary>
/// <param name="gpaService">The injected service that handles the actual GPA math.</param>
// These attributes tell .NET that this class handles API traffic and routes it to /api/gpa
[ApiController]
[Route("api/[controller]")]
public class GpaController(IGpaCalculatorService gpaService) : ControllerBase
{
    private readonly IGpaCalculatorService _gpaService = gpaService;


    /// <summary>
    /// Calculates a cumulative GPA based on a provided list of courses, credits, and grades.
    /// </summary>
    /// <param name="request">The payload from the React frontend containing the course list.</param>
    /// <returns>An HTTP response containing the calculated GPA and a status message.</returns>
    /// <response code="200">Returns the successfully calculated GPA result.</response>
    /// <response code="400">If the incoming JSON request is malformed or missing required fields.</response>
    [HttpPost("calculate-gpa")]
    public ActionResult<GpaCalculationResponse> CalculateGpa([FromBody] GpaCalculationRequest request)
    {
        return Ok(_gpaService.CalculateGpa(request));
    }

    /// <summary>
    /// Calculates the required GPA for a future block of credits to achieve a specific target GPA.
    /// </summary>
    /// <param name="request">The payload containing current GPA, past credits, target GPA, and future credits.</param>
    /// <returns>An HTTP response containing the required GPA and a status message.</returns>
    /// <response code="200">Returns the successfully calculated target GPA result.</response>
    /// <response code="400">If the incoming JSON request is malformed.</response>
    [HttpPost("calculate-target")]
    public ActionResult<TargetGpaResponse> CalculateTargetGpa([FromBody] TargetGpaRequest request)
    {
        return Ok(_gpaService.CalculateTargetGpa(request));
    }
}
