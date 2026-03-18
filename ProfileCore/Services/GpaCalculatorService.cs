using ProfileCore.Models;

namespace ProfileCore.Services;

/// <inheritdoc />
public class GpaCalculatorService : IGpaCalculatorService
{
    /// <inheritdoc />
    public GpaCalculationResponse CalculateGpa(GpaCalculationRequest request)
    {
        double CalculatedGpa = 0;
        double TotalCreditHours = 0;
        string Message = "Please add at least one course with more than 0 credit hours.";

        if (request.Courses.Count > 0)
        {
            double totalGradePoints = 0;
            TotalCreditHours = 0;

            foreach (var course in request.Courses)
            {
                totalGradePoints += ConvertGradeToPoints(course.Grade) * course.CreditHours;
                TotalCreditHours += course.CreditHours;
            }

            if (TotalCreditHours > 0)
            {
                if (request.CurrentGpa.HasValue && request.PastCreditHours.HasValue)
                {
                    totalGradePoints += request.CurrentGpa.Value * request.PastCreditHours.Value;
                    TotalCreditHours += request.PastCreditHours.Value;
                }

                CalculatedGpa = Math.Round(totalGradePoints / TotalCreditHours, 2);
                
                Message = "GPA successfully calculated!";
            }
            else if (request.CurrentGpa.HasValue)
            {
                CalculatedGpa = request.CurrentGpa.Value;
            }
        } 
        else if (request.CurrentGpa.HasValue)
        {
            CalculatedGpa = request.CurrentGpa.Value;
            if (request.PastCreditHours.HasValue){
                TotalCreditHours = request.PastCreditHours.Value;
            }
        }
        
        return  new GpaCalculationResponse(CalculatedGpa, Message, TotalCreditHours);
    }

    /// <inheritdoc />
    public TargetGpaResponse CalculateTargetGpa(TargetGpaRequest request)
    {
        double requiredGpa = -1;
        string message = "Need at least one credit hour";
        if (request.NewCreditHours > 0)
        {
            requiredGpa = (request.TargetGpa * (request.PastCreditHours + request.NewCreditHours) - request.CurrentGpa * request.PastCreditHours) / request.NewCreditHours;
            requiredGpa = Math.Round(requiredGpa, 2);
            
            message = $"You will need a minimum {requiredGpa} GPA in your {request.NewCreditHours} credit hours.\n";
            message += requiredGpa switch
            {
                > 4.0 => "Try adding more classes!",
                >= 3.5 => "You will need mostly A's to pull this off.",
                >= 2.0 => "You will need a solid mix of A's, B's, and maybe some C's.",
                _ => "You have plenty of breathing room to hit this target."
            };

        }
        
        return new TargetGpaResponse(requiredGpa, message);
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