using ProfileCore.Models;

namespace ProfileCore.Services;

/// <inheritdoc />
public class GpaCalculatorService : IGpaCalculatorService
{
    /// <inheritdoc />
    public GpaCalculationResponse CalculateGpa(GpaCalculationRequest request)
    {
        double CalculatedGpa = 0;
        string Message = "Please add at least one course with more than 0 credit hours.";

        if (request.Courses.Count > 0)
        {
            double totalGradePoints = 0;
            double totalCreditHours = 0;

            foreach (var course in request.Courses)
            {
                totalGradePoints += ConvertGradeToPoints(course.Grade) * course.CreditHours;
                totalCreditHours += course.CreditHours;
            }

            if (totalCreditHours > 0)
            {
                if (request.CurrentGpa.HasValue && request.PastCreditHours.HasValue)
                {
                    totalGradePoints += request.CurrentGpa.Value * request.PastCreditHours.Value;
                    totalCreditHours += request.PastCreditHours.Value;
                }

                CalculatedGpa = Math.Round(totalGradePoints / totalCreditHours, 2);
                
                Message = "GPA successfully calculated!";
            }
            else if (request.CurrentGpa.HasValue)
            {
                CalculatedGpa = request.CurrentGpa.Value;
            }
        } 
        
        return  new GpaCalculationResponse(CalculatedGpa, Message);
    }

    /// <summary>
    /// Converts a alphabetical letter grade into its corresponding numerical point value.
    /// </summary>
    /// <param name="grade">A string containing the letter grade</param>
    /// <returns>The numerical point equivalent on a 4.0 scale or 0.0 if the grade is unrecognized.</returns>
    private static double ConvertGradeToPoints(string grade)
    {
        return grade.ToUpper() switch
        {
            "A+" or "A" => 4.0,
            "A-" => 3.7,
            "B+" => 3.3,
            "B" => 3.0,
            "B-" => 2.7,
            "C+" => 2.3,
            "C" => 2.0,
            "C-" => 1.7,
            "D+" => 1.3,
            "D" => 1.0,
            "F" => 0.0,
            _ => 0.0
        };        
    }
 }