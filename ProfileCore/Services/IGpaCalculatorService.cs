using ProfileCore.Models;

namespace ProfileCore.Services;

/// <summary>
/// Service responseible for handling all Grade Point Average (GPA) calculations.
/// Evaluates lists of academic courses to compute cumulative GPA on a 4.0 scale.
/// </summary>
public interface IGpaCalculatorService
{
    /// <summary>
    /// Calculates the overall GPA from a provided list of courses, using their credit hours and letter grades.
    /// </summary>
    /// <param name="request">The payload containing the list of <see cref="Course"/> objects to be evaluated.</param>
    /// <returns>A <see cref="GpaCalculationResponse"/> containing the calcuated GPA and a status message.</returns>
    GpaCalculationResponse CalculateGpa(GpaCalculationRequest request);

    /// <summary>
    /// Calculates the minimum required GPA to achieve a target GAP using their credit hours.
    /// </summary>
    /// <param name="request">The payload containing the target gpa, current gpa and past and future credit hours.</param>
    /// <returns>A <see cref="GpaCalculationResponse"/> containing the calcuated required minimum GPA to achieve the target and a status message.</returns>
    public TargetGpaResponse CalculateTargetGpa(TargetGpaRequest request);
}
