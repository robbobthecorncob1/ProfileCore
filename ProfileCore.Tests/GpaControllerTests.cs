using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileCore.Controllers;
using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Tests;

public class GpaControllerTests
{
    private readonly Mock<IGpaCalculatorService> _mockGpaService;
    private readonly GpaController _sut;

    public GpaControllerTests()
    {
        _mockGpaService = new Mock<IGpaCalculatorService>();
        
        _sut = new GpaController(_mockGpaService.Object);
    }

    [Fact]
    public void CalculateGpa_ValidRequest_ReturnsOkObjectResult()
    {
        var request = new GpaCalculationRequest([], null, null);
        
        var expectedResponse = new GpaCalculationResponse(3.8, "Mocked success!");
        _mockGpaService.Setup(service => service.CalculateGpa(request)).Returns(expectedResponse);

        var result = _sut.CalculateGpa(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
 
        var returnValue = Assert.IsType<GpaCalculationResponse>(okResult.Value);
        Assert.Equal(3.8, returnValue.CalculatedGpa);
        Assert.Equal("Mocked success!", returnValue.Message);
    }

    [Fact]
    public void CalculateGpa_ServiceReturnsWarning_ReturnsOkObjectResultWithWarning()
    {
        var request = new GpaCalculationRequest([], null, null);
        var expectedResponse = new GpaCalculationResponse(0, "Please add at least one course with more than 0 credit hours.");
        
        _mockGpaService
            .Setup(service => service.CalculateGpa(request))
            .Returns(expectedResponse);

        var result = _sut.CalculateGpa(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<GpaCalculationResponse>(okResult.Value);
        Assert.Equal(0, returnValue.CalculatedGpa);
        Assert.Equal("Please add at least one course with more than 0 credit hours.", returnValue.Message);
    }

    [Fact]
    public void CalculateGpa_Always_CallsServiceExactlyOnce()
    {
        var request = new GpaCalculationRequest([new Course("Math", 3.0, "A")], null, null);
        var expectedResponse = new GpaCalculationResponse(4.0, "GPA successfully calculated!");
        
        _mockGpaService
            .Setup(service => service.CalculateGpa(It.IsAny<GpaCalculationRequest>()))
            .Returns(expectedResponse);

        _sut.CalculateGpa(request);

        _mockGpaService.Verify(service => service.CalculateGpa(request), Times.Once);
    }

    [Fact]
    public void CalculateGpa_ServiceThrowsException_LetsExceptionBubbleUp()
    {
        var request = new GpaCalculationRequest([], null, null);
        
        _mockGpaService
            .Setup(service => service.CalculateGpa(It.IsAny<GpaCalculationRequest>()))
            .Throws(new InvalidOperationException("Critical service failure."));

        Assert.Throws<InvalidOperationException>(() => _sut.CalculateGpa(request));
    }

    [Fact]
    public void CalculateTargetGpa_ValidRequest_ReturnsOkObjectResult()
    {
        var request = new TargetGpaRequest(3.0, 60.0, 3.5, 15.0);
        var expectedResponse = new TargetGpaResponse(3.5, "You will need a minimum 3.5 GPA in your 15 credit hours.\nYou will need mostly A's to pull this off.");
        
        _mockGpaService
            .Setup(service => service.CalculateTargetGpa(request))
            .Returns(expectedResponse);

        var result = _sut.CalculateTargetGpa(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<TargetGpaResponse>(okResult.Value);
        Assert.Equal(3.5, returnValue.RequiredGpa);
        Assert.Equal("You will need a minimum 3.5 GPA in your 15 credit hours.\nYou will need mostly A's to pull this off.", returnValue.Message);
    }

    [Fact]
    public void CalculateTargetGpa_Always_CallsServiceExactlyOnce()
    {
        var request = new TargetGpaRequest(2.5, 45.0, 3.0, 15.0);
        var expectedResponse = new TargetGpaResponse(3.0, "You will need a minimum 3.0 GPA in your 15 credit hours.");
        
        _mockGpaService
            .Setup(service => service.CalculateTargetGpa(It.IsAny<TargetGpaRequest>()))
            .Returns(expectedResponse);

        _sut.CalculateTargetGpa(request);

        _mockGpaService.Verify(service => service.CalculateTargetGpa(request), Times.Once);
    }

    [Fact]
    public void CalculateTargetGpa_ServiceThrowsException_LetsExceptionBubbleUp()
    {
        var request = new TargetGpaRequest(3.0, 60.0, 3.5, 15.0);
        
        _mockGpaService
            .Setup(service => service.CalculateTargetGpa(It.IsAny<TargetGpaRequest>()))
            .Throws(new InvalidOperationException("Critical target service failure."));

        Assert.Throws<InvalidOperationException>(() => _sut.CalculateTargetGpa(request));
    }
}