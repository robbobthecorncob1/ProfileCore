using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Tests;

public class GpaCalculatorServiceTests
{
    private readonly GpaCalculatorService _sut;

    public GpaCalculatorServiceTests()
    {
        _sut = new GpaCalculatorService();
    }

    [Fact]
    public void CalculateGpa_WithValidCourses_ReturnsCorrectGpa()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Math", 3.0, "A"),
                new Course("Science", 4.0, "B")
            ]  
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(3.43, result.CalculatedGpa);
    }

    [Fact]
    public void CalculateGpa_WithEmptyCourseList_ReturnsZeroAndErrorMessage()
    {
        // Arrange: Send a completely empty list
        var request = new GpaCalculationRequest([]);

        // Act
        var result = _sut.CalculateGpa(request);

        // Assert: Prove it caught the error
        Assert.Equal(0, result.CalculatedGpa);
        Assert.Equal("Please add at least one course.", result.Message);
    }

    [Fact]
    public void CalculateGpa_WithZeroTotalCreditHours_ReturnsZeroAndErrorMessage()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Gym", 0, "A")
            ]
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(0, result.CalculatedGpa);
        Assert.Equal("Total credit hours cannot be 0. Please enter credit hours.", result.Message);
    }
}