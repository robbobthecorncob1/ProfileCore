namespace ProfileCore.Models;

/// <summary>
/// Represents the incoming payload from the client containing the data needed to calculate a GPA.
/// </summary>
/// <param name="Courses">A list of academic courses to be used in the calculation.</param>
/// <param name="CurrentGpa">The user's current GPA number</param>
/// <param name="PastCreditHours">The number of credit hours the user has taken in the past.</param>
public record GpaCalculationRequest(
    List<Course> Courses,
    double? CurrentGpa,
    double? PastCreditHours
);

/// <summary>
/// Represents an academic course and its grading information.
/// </summary>
/// <param name="CourseName">The name or identifier of the course.</param>
/// <param name="CreditHours">The number of credit hours or units the course is worth.</param>
/// <param name="Grade">The letter grade achieved in the course.</param>
public record Course(
    string CourseName,
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

/// <summary>
/// Represents the incoming payload from the client containing the data needed to calculate the minimum GPA.
/// </summary>
/// <param name="CurrentGpa">The user's current GPA number</param>
/// <param name="PastCreditHours">The number of credit hours the user has taken in the past.</param>
/// <param name="TargetGpa">The user's target GPA number to achieve.</param>
/// <param name="NewCreditHours">The next block of credit hours the student will complete.</param>
public record TargetGpaRequest(
    double CurrentGpa,
    double PastCreditHours,
    double TargetGpa,
    double NewCreditHours
);

/// <summary>
/// The final result of a GPA calculation, containing the required minimum GPA to the client.
/// </summary>
/// <param name="RequiredGpa">The computed Grade Point Average.</param>
/// <param name="Message">A message containing information about the calculation.</param>
public record TargetGpaResponse(
    double RequiredGpa,
    string Message
);