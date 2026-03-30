using Microsoft.Extensions.Configuration;
using Moq;
using ProfileCore.Models.website;
using ProfileCore.Services;
using Resend;

namespace ProfileCore.Tests.Website;

public class EmailServiceTests
{
    private readonly Mock<IResend> _mockResend = new();
    private readonly Mock<IConfiguration> _mockConfig = new();
    private readonly EmailService _sut;

    public EmailServiceTests()
    {
        _mockConfig.Setup(config => config["Resend:FromEmail"]).Returns("from@test.com");
        _mockConfig.Setup(config => config["Resend:ToEmail"]).Returns("to@test.com");

        _sut = new EmailService(_mockResend.Object, _mockConfig.Object);
    }

    [Fact]
    public async Task SendContactEmailAsync_ValidSubmission()
    {
        await _sut.SendContactEmailAsync(new ContactSubmission("Jane Doe", "jane@example.com", "Job Inquiry", "Hello!\nI want to hire you."));

        _mockResend.Verify(
            resend => resend.EmailSendAsync(
                It.Is<EmailMessage>(msg => 
                    msg.From.Email == "from@test.com" &&
                    msg.To.Contains("to@test.com") &&
                    msg.Subject == "Portfolio Contact: Job Inquiry" &&
                    msg.HtmlBody!.Contains("jane@example.com") &&
                    msg.HtmlBody.Contains("Jane Doe") &&
                    msg.HtmlBody.Contains("Hello!<br/>I want to hire you.")
                ), 
                It.IsAny<CancellationToken>()
            ), 
            Times.Once
        );
    }
}