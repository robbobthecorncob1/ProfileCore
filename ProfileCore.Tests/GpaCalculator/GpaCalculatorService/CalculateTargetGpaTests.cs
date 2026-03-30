using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Tests.GpaCalculator;

public class CalculateTargetGpaTests
{
    private readonly GpaCalculatorService _sut = new();
    
    public const string Above4GpaMsg = "It is currently not possible. Try adding more classes!";
    public const string AtAbove3dot5GpaMsg = "You will need mostly A's to pull this off.";
    public const string AtAbove2GpaMsg = "You will need a solid mix of A's, B's, and maybe some C's.";
    public const string At0GpaMsg = "You could fail every class and still hit your goal!";
    public const string Below2GpaMsg = "You have plenty of breathing room to hit this target.";
    public const string ZeroNewCreditsMsg = "Need at least one credit hour";

    /// <param name="currentGpa"></param>
    /// <param name="pastCredits"></param>
    /// <param name="targetGpa"></param>
    /// <param name="newCredits"></param>
    /// <param name="expectedRequiredGpa"></param>
    /// <param name="expectedMessageSuffix"></param>
    [Theory] 
    [InlineData(0.0, 0.0, 0.0, 0.0, -1.0, ZeroNewCreditsMsg)]
    [InlineData(2.0, 15.0, 3.25, 15.0, 4.5, Above4GpaMsg)]
    [InlineData(3.5, 15.0, 3.5, 15.0, 3.5, AtAbove3dot5GpaMsg)]
    [InlineData(2.0, 15.0, 2.0, 15.0, 2.0, AtAbove2GpaMsg)]
    [InlineData(3.0, 15.0, 2.0, 15.0, 1.0, Below2GpaMsg)]
    [InlineData(4.0, 15.0, 2.0, 15.0, 0.0, At0GpaMsg)]
    public void CalculateTargetGpa_VariousInputs_ReturnsExpectedResults(
        double currentGpa, 
        double pastCredits, 
        double targetGpa, 
        double newCredits, 
        double expectedRequiredGpa, 
        string expectedMessageSuffix
    )
    {
        var result = _sut.CalculateTargetGpa(new TargetGpaRequest(currentGpa, pastCredits, targetGpa, newCredits));

        Assert.Equal(expectedRequiredGpa, result.RequiredGpa);

        if (newCredits > 0) Assert.EndsWith(expectedMessageSuffix, result.Message);
        else Assert.Equal(expectedMessageSuffix, result.Message);
    }
}