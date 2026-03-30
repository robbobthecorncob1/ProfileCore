using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileCore.Controllers;
using ProfileCore.Models.website;
using ProfileCore.Services;

namespace ProfileCore.Tests.Website;

public class WebsiteControllerTests
{
    private readonly Mock<IWebsiteService> _mockWebsiteService = new();
    private readonly Mock<IEmailService> _mockEmailService = new();
    private readonly WebsiteController _sut;

    public WebsiteControllerTests()
    {
        _sut = new WebsiteController(_mockWebsiteService.Object, _mockEmailService.Object);
    }

    [Fact]
    public async Task GetProfile_ReturnsOkObjectResult_WithProfileData()
    {
        _mockWebsiteService.Setup(service => service.GetProfileAsync()).ReturnsAsync(new Profile(1, "First", "Last", "Dev", "Bio", null, ["C#"]));
        Assert.Equal("First", Assert.IsType<Profile>(Assert.IsType<OkObjectResult>((await _sut.GetProfile()).Result).Value).FirstName);
    }

    [Fact]
    public async Task GetWorkExperience_ReturnsOkObjectResult_WithJobsList()
    {
        _mockWebsiteService.Setup(service => service.GetWorkExperienceAsync()).ReturnsAsync([]);
        Assert.IsType<List<Job>>(Assert.IsType<OkObjectResult>((await _sut.GetWorkExperience()).Result).Value);
    }

    [Fact]
    public async Task GetProjects_ReturnsOkObjectResult_WithProjectsList()
    {
        _mockWebsiteService.Setup(service => service.GetProjectsAsync()).ReturnsAsync([]);
        Assert.IsType<List<Project>>(Assert.IsType<OkObjectResult>((await _sut.GetProjects()).Result).Value);
    }

    [Fact]
    public async Task GetSkills_ReturnsOkObjectResult_WithSkillsList()
    {
        _mockWebsiteService.Setup(service => service.GetSkillsAsync()).ReturnsAsync([]);
        Assert.IsType<List<Skill>>(Assert.IsType<OkObjectResult>((await _sut.GetSkills()).Result).Value);
    }

    [Fact]
    public async Task GetEducation_ReturnsOkObjectResult_WithEducationList()
    {
        _mockWebsiteService.Setup(service => service.GetEducationAsync()).ReturnsAsync([]);
        Assert.IsType<List<EducationProgram>>(Assert.IsType<OkObjectResult>(( await _sut.GetEducation()).Result).Value);
    }

    [Fact]
    public async Task GetCourses_ReturnsOkObjectResult_WithCoursesList()
    {
        _mockWebsiteService.Setup(service => service.GetCoursesAsync()).ReturnsAsync([]);
        Assert.IsType<List<Course>>(Assert.IsType<OkObjectResult>((await _sut.GetCourses()).Result).Value);
    }

    [Fact]
    public async Task GetSystemStatus_ReturnsOkObjectResult_WithStatus()
    {
        _mockWebsiteService.Setup(service => service.GetSystemStatusAsync()).ReturnsAsync(new SystemStatus("Test", "1.0", DateTime.UtcNow, 100, "Operational", "Operational"));
        Assert.IsType<SystemStatus>(Assert.IsType<OkObjectResult>((await _sut.GetSystemStatus()).Result).Value);
    }


    [Fact]
    public async Task HandleContactSubmission_ValidData_CallsEmailServiceAndReturnsOk()
    {
        var submission = new ContactSubmission("Jane", "jane@test.com", "Hi", "Message");
        
        _mockEmailService.Setup(service => service.SendContactEmailAsync(submission)).Returns(Task.CompletedTask);

        Assert.IsType<OkObjectResult>(await _sut.HandleContactSubmission(submission));
        
        _mockEmailService.Verify(service => service.SendContactEmailAsync(submission), Times.Once);
    }

    [Theory]
    [InlineData("", "Valid Message")]
    [InlineData("test@test.com", "")]
    [InlineData(null, "Valid Message")]
    [InlineData("test@test.com", null)]
    [InlineData("   ", "Valid Message")]
    public async Task HandleContactSubmission_MissingEmailOrMessage(string? email, string? message)
    {
        Assert.Equal("Email and Message are required.", Assert.IsType<BadRequestObjectResult>(await _sut.HandleContactSubmission(new ContactSubmission("Jane", email!, "Subject", message!))).Value);
        _mockEmailService.Verify(service => service.SendContactEmailAsync(It.IsAny<ContactSubmission>()), Times.Never);
    }

    [Fact]
    public async Task HandleContactSubmission_EmailServiceThrowsException()
    {
        _mockEmailService.Setup(service => service.SendContactEmailAsync(It.IsAny<ContactSubmission>())).ThrowsAsync(new Exception("Simulated SMTP server failure"));

        var objectResult = Assert.IsType<ObjectResult>(await _sut.HandleContactSubmission(new ContactSubmission("Jane", "jane@test.com", "Hi", "Message")));
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("An error occurred while sending the email.", objectResult.Value);
    }
}