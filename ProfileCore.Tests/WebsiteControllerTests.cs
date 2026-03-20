using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileCore.Controllers;
using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Tests;

public class WebsiteControllerTests
{
    private readonly Mock<IWebsiteService> _mockService = new();
    private readonly WebsiteController _controller;

    public WebsiteControllerTests()
    {
        _controller = new WebsiteController(_mockService.Object);
    }

    [Fact]
    public async Task GetProfile_ReturnsOkObjectResult_WithProfileData()
    {
        var expectedProfile = new Profile(1, "First", "Last", "Dev", "Bio", null, ["C#"]);
        _mockService.Setup(service => service.GetProfileAsync()).ReturnsAsync(expectedProfile);

        var actionResult = await _controller.GetProfile();

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedProfile = Assert.IsType<Profile>(okResult.Value);
        
        Assert.Equal("First", returnedProfile.FirstName);
    }

    [Fact]
    public async Task GetExperience_ReturnsOkObjectResult_WithWorkExperience()
    {
        var expectedExperience = new WorkExperience([]);
        _mockService.Setup(service => service.GetWorkExperienceAsync()).ReturnsAsync(expectedExperience);

        var actionResult = await _controller.GetExperience();

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.IsType<WorkExperience>(okResult.Value);
    }

    [Fact]
    public async Task GetSystemStatus_ReturnsOkObjectResult_WithStatus()
    {
        var expectedStatus = new SystemStatus("Test", "1.0", DateTime.UtcNow, 100, "Healthy");
        _mockService.Setup(service => service.GetSystemStatusAsync()).ReturnsAsync(expectedStatus);

        var actionResult = await _controller.GetSystemStatus();

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedStatus = Assert.IsType<SystemStatus>(okResult.Value);
        
        Assert.Equal("Healthy", returnedStatus.StatusMessage);
    }

    [Fact]
    public async Task SubmitContactForm_WithValidData_ReturnsOkResult()
    {
        var submission = new ContactSubmission("Jane", "jane@test.com", "Hi", "Message", DateTime.UtcNow);
        
        _mockService.Setup(service => service.ProcessContactSubmissionAsync(submission))
                    .Returns(Task.CompletedTask);

        var actionResult = await _controller.SubmitContactForm(submission);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task SubmitContactForm_WithInvalidModelState_ReturnsBadRequest()
    {
        var submission = new ContactSubmission("", "invalid-email", "", "", DateTime.UtcNow);
        
        _controller.ModelState.AddModelError("Email", "The Email field is not a valid e-mail address.");

        var actionResult = await _controller.SubmitContactForm(submission);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        Assert.Equal(400, badRequestResult.StatusCode);
        
        _mockService.Verify(service => service.ProcessContactSubmissionAsync(It.IsAny<ContactSubmission>()), Times.Never);
    }
}