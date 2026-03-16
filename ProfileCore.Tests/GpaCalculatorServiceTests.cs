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
    public void CalculateGpa_WithValidCourses_NoPastHistory_ReturnsCorrectGpa()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Math", 3.0, "A"),
                new Course("Science", 4.0, "B")
            ],
            null,
            null
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(3.43, result.CalculatedGpa);
        Assert.Equal("GPA successfully calculated!", result.Message);
    }

    [Fact]
    public void CalculateGpa_WithValidCourses_WithPastHistory_ReturnsCumulativeGpa()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("History", 3.0, "A"),
                new Course("Art", 3.0, "A")
            ],
            3.0,
            60.0
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(3.09, result.CalculatedGpa);
        Assert.Equal("GPA successfully calculated!", result.Message);
    }

    [Fact]
    public void CalculateGpa_WithEmptyCourseList_ReturnsZeroAndDefaultMessage()
    {
        var request = new GpaCalculationRequest([], null, null);

        var result = _sut.CalculateGpa(request);

        Assert.Equal(0, result.CalculatedGpa);
        Assert.Equal("Please add at least one course with more than 0 credit hours.", result.Message);
    }

    [Fact]
    public void CalculateGpa_WithZeroTotalCreditHours_ReturnsZeroAndDefaultMessage()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Gym", 0, "A"),
                new Course("Study Hall", 0, "B")
            ],
            null,
            null
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(0, result.CalculatedGpa);
        Assert.Equal("Please add at least one course with more than 0 credit hours.", result.Message);
    }

    [Fact]
    public void CalculateGpa_WithZeroTotalCreditHours_ButHasPastGpa_ReturnsPastGpa()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Gym", 0, "A")
            ],
            3.5,
            60.0
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(3.5, result.CalculatedGpa);
        Assert.Equal("Please add at least one course with more than 0 credit hours.", result.Message);
    }

    [Fact]
    public void CalculateGpa_WithLowercaseAndMixedGrades_CalculatesCorrectly()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Math", 3.0, "a+"),
                new Course("Science", 3.0, "b-"),
                new Course("English", 3.0, "c")
            ],
            null,
            null
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(2.9, result.CalculatedGpa);
    }

    [Fact]
    public void CalculateGpa_WithUnrecognizedGrades_TreatsAsZeroPoints()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Math", 3.0, "Z"),
                new Course("Science", 3.0, "Pass")
            ],
            null,
            null
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(0.0, result.CalculatedGpa);
    }

    [Fact]
    public void CalculateGpa_WithPartialPastHistory_IgnoresPastHistory()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Math", 3.0, "B") 
            ],
            3.8,
            null 
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(3.0, result.CalculatedGpa);
    }

    [Fact]
    public void CalculateGpa_WithMassivePastHistory_DilutesNewGradesCorrectly()
    {
        var request = new GpaCalculationRequest(
            [
                new Course("Math", 3.0, "F") 
            ],
            4.0,
            1000.0
        );

        var result = _sut.CalculateGpa(request);

        Assert.Equal(3.99, result.CalculatedGpa);
    }
}