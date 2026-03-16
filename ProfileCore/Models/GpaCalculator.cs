namespace ProfileCore.Models;

/// <summary>
/// Represents the incoming payload from the client containing the data needed to calculate a GPA.
/// </summary>
/// <param name="Courses">A list of academic courses to be used in the calculation.</param>
public record GpaCalculationRequest(
    List<Course> Courses
);

/// <summary>
/// Represents an academic course and its grading information.
/// </summary>
/// <param name="ClassName">The name or identifier of the course.</param>
/// <param name="CreditHours">The number of credit hours or units the course is worth.</param>
/// <param name="Grade">The letter grade achieved in the course.</param>
public record Course(
    string ClassName,
    double CreditHours, 
    string Grade
);

/// <summary>
/// The final result of a GPA calculation sent back to the client
/// </summary>
/// <param name="CalculatedGpa">The computed Grade Point Average.</param>
/// <param name="Message">A message containing information about the calculation.</param>
public record GpaCalculationResponse(
    double CalculatedGpa,
    string Message
);
