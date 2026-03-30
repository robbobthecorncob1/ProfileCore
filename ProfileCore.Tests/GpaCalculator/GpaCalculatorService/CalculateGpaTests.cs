using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Tests.GpaCalculator;

public class CalculateGpaTests
{
    private readonly GpaCalculatorService _sut = new();
    private readonly string _calculationSuccessMsg = "GPA successfully calculated!";
    private readonly string _addOneCourseMsg = "Please add at least one course with more than 0 credit hours.";

    /// <param name="courseCredits"></param>
    /// <param name="pastGpa"></param>
    /// <param name="pastCredits"></param>
    /// <param name="expectedGpa"></param>
    /// <param name="expectSuccessMsg"></param>
    /// <param name="expectedTotalCredits"></param>
    [Theory]
    // EMPTY COURSE LIST CASES (courseCredits = -1)
    [InlineData(-1.0, null, null, 0.0, false, 0.0)]
    [InlineData(-1.0, 0.0, null, 0.0, false, 0.0)]
    [InlineData(-1.0, 1.0, null, 0.0, false, 0.0)]
    [InlineData(-1.0, 1.1, null, 0.0, false, 0.0)]
    [InlineData(-1.0, 4.0, null, 0.0, false, 0.0)]
    [InlineData(-1.0, 4.1, null, 0.0, false, 0.0)]
    [InlineData(-1.0, null, 0.0, 0.0, false, 0.0)]
    [InlineData(-1.0, null, 5.0, 0.0, false, 0.0)]
    [InlineData(-1.0, null, 10.0, 0.0, false, 0.0)]
    [InlineData(-1.0, 0.0, 0.0, 0.0, false, 0.0)]
    [InlineData(-1.0, 1.0, 1.0, 1.0, false, 1.0)] 
    
    // --- ONE COURSE, 0 CREDITS (courseCredits = 0) ---
    [InlineData(0.0, null, null, 0.0, false, 0.0)]
    [InlineData(0.0, 0.0, null, 0.0, false, 0.0)]
    [InlineData(0.0, null, 0.0, 0.0, false, 0.0)]
    [InlineData(0.0, 0.0, 0.0, 0.0, false, 0.0)]
    [InlineData(0.0, 1.0, null, 0.0, false, 0.0)]
    [InlineData(0.0, null, 1.0, 0.0, false, 0.0)]
    [InlineData(0.0, 0.0, 1.0, 0.0, true, 1.0)] 
    [InlineData(0.0, 1.0, 0.0, 0.0, false, 0.0)]
    [InlineData(0.0, 1.0, 1.0, 1.0, true, 1.0)]

    // --- ONE COURSE, 1 CREDIT (courseCredits = 1) ---
    [InlineData(1.0, null, null, 4.0, true, 1.0)]
    [InlineData(1.0, 0.0, null, 4.0, true, 1.0)]
    [InlineData(1.0, null, 0.0, 4.0, true, 1.0)]
    [InlineData(1.0, 0.0, 0.0, 4.0, true, 1.0)]
    [InlineData(1.0, 1.0, null, 4.0, true, 1.0)]
    [InlineData(1.0, null, 1.0, 4.0, true, 1.0)]
    [InlineData(1.0, 0.0, 1.0, 2.0, true, 2.0)]
    [InlineData(1.0, 1.0, 0.0, 4.0, true, 1.0)]
    [InlineData(1.0, 1.0, 1.0, 2.5, true, 2.0)]
    public void CalculateGpa_VariousInputs_ReturnsExpectedResults(
        double courseCredits, 
        double? pastGpa, 
        double? pastCredits, 
        double expectedGpa, 
        bool expectSuccessMsg, 
        double expectedTotalCredits)
    {
        var courses = courseCredits == -1 ? [] : new List<Course> { new("", courseCredits, "A") };
        
        var expectedMessage = expectSuccessMsg ? _calculationSuccessMsg : _addOneCourseMsg;

        var result = _sut.CalculateGpa(new GpaCalculationRequest(courses, pastGpa, pastCredits));

        Assert.Equal(expectedGpa, result.CalculatedGpa);
        Assert.Equal(expectedMessage, result.Message);
        Assert.Equal(expectedTotalCredits, result.TotalCreditHours);
    }
}