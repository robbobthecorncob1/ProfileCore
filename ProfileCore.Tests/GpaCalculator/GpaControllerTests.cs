using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileCore.Controllers;
using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Tests.GpaCalculator;

public class GpaControllerTests
{
    private readonly Mock<IGpaCalculatorService> _mockGpaService = new();
    private readonly GpaController _sut;

    public GpaControllerTests()
    {   
        _sut = new GpaController(_mockGpaService.Object);
    }

    [Fact]
    public void CalculateGpa_ValidRequest()
    {
        var request = new GpaCalculationRequest([], null, null);
        var expectedResponse = new GpaCalculationResponse(3.8, "Mocked success!", 0);
        
        _mockGpaService.Setup(service => service.CalculateGpa(request)).Returns(expectedResponse);

        var result = _sut.CalculateGpa(request);
        
        Assert.Equal(expectedResponse.CalculatedGpa, Assert.IsType<GpaCalculationResponse>(Assert.IsType<OkObjectResult>(result.Result).Value).CalculatedGpa);
        _mockGpaService.Verify(service => service.CalculateGpa(request), Times.Once);
    }

    [Fact]
    public void CalculateGpa_ServiceThrowsException()
    {
        _mockGpaService.Setup(service => service.CalculateGpa(It.IsAny<GpaCalculationRequest>())).Throws(new InvalidOperationException("Critical service failure."));

        Assert.Throws<InvalidOperationException>(() => _sut.CalculateGpa(new GpaCalculationRequest([], null, null)));
    }

    [Fact]
    public void CalculateTargetGpa_ValidRequest()
    {
        var request = new TargetGpaRequest(3.0, 60.0, 3.5, 15.0);
        var expectedResponse = new TargetGpaResponse(3.5, "You will need a minimum 3.5 GPA in your 15 credit hours.\nYou will need mostly A's to pull this off.");
        
        _mockGpaService.Setup(service => service.CalculateTargetGpa(request)).Returns(expectedResponse);

        var result = _sut.CalculateTargetGpa(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        var returnValue = Assert.IsType<TargetGpaResponse>(okResult.Value);
        Assert.Equal(expectedResponse.RequiredGpa, returnValue.RequiredGpa);
        Assert.Equal(expectedResponse.Message, returnValue.Message);
        
        _mockGpaService.Verify(service => service.CalculateTargetGpa(request), Times.Once);
    }

    [Fact]
    public void CalculateTargetGpa_ServiceThrowsException()
    {
        _mockGpaService.Setup(service => service.CalculateTargetGpa(It.IsAny<TargetGpaRequest>())).Throws(new InvalidOperationException("Critical target service failure."));

        Assert.Throws<InvalidOperationException>(() => _sut.CalculateTargetGpa(new TargetGpaRequest(3.0, 60.0, 3.5, 15.0)));
    }
}