using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProfileCore.Data;
using ProfileCore.Models;
using ProfileCore.Services;

namespace ProfileCore.Tests;

public class WebsiteServiceTests
{
    /// <summary>
    /// Helper method to generate a fresh, empty In-Memory database for each test.
    /// We use a unique Guid for the database name so tests running in parallel don't step on each other.
    /// </summary>
    private WebsiteDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<WebsiteDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new WebsiteDbContext(options);
    }

    [Fact]
    public async Task GetProfileAsync_WhenDatabaseIsEmpty_ThrowsInvalidOperationException()
    {
        var context = CreateInMemoryDbContext();
        var mockLogger = new Mock<ILogger<WebsiteService>>();
        var service = new WebsiteService(context, mockLogger.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetProfileAsync());
        Assert.Equal("Profile data has not been seeded into the database yet.", exception.Message);
    }

    [Fact]
    public async Task GetProfileAsync_WhenProfileExists_ReturnsProfileWithSocialLinks()
    {
        var context = CreateInMemoryDbContext();
        var mockLogger = new Mock<ILogger<WebsiteService>>();
        
        var fakeProfile = new Profile(1, "First", "Last", "Headline", "Bio", "url", ["C#", "SQL"])
        {
            SocialLinks = [new SocialLink(1, "GitHub", "https://github.com")]
        };
        context.Profile.Add(fakeProfile);
        await context.SaveChangesAsync();

        var service = new WebsiteService(context, mockLogger.Object);

        var result = await service.GetProfileAsync();

        Assert.NotNull(result);
        Assert.Equal("First", result.FirstName);
        Assert.Single(result.SocialLinks);
        Assert.Equal("GitHub", result.SocialLinks[0].Platform);
    }

    [Fact]
    public async Task GetWorkExperienceAsync_ReturnsWrappedJobsList()
    {
        var context = CreateInMemoryDbContext();
        var mockLogger = new Mock<ILogger<WebsiteService>>();
        
        context.Jobs.Add(new Job(1, true, "Dev", "Corp", [".NET"], "Jan 2020", null, "Did stuff"));
        context.Jobs.Add(new Job(2, false, "Intern", "Startup", ["JS"], "Jan 2019", "Dec 2019", "Learned stuff"));
        await context.SaveChangesAsync();

        var service = new WebsiteService(context, mockLogger.Object);

        var result = await service.GetWorkExperienceAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Jobs.Count);
    }

    [Fact]
    public async Task GetSystemStatusAsync_ReturnsValidStatusRecord()
    {
        var context = CreateInMemoryDbContext();
        var mockLogger = new Mock<ILogger<WebsiteService>>();
        var service = new WebsiteService(context, mockLogger.Object);

        var result = await service.GetSystemStatusAsync();

        Assert.NotNull(result);
        Assert.Equal("1.0.0", result.Version);
        Assert.Equal("API and Database are online and healthy.", result.StatusMessage);
        Assert.True(result.UptimeMilliseconds >= 0);
    }

    [Fact]
    public async Task ProcessContactSubmissionAsync_ExecutesWithoutExceptions()
    {
        var context = CreateInMemoryDbContext();
        var mockLogger = new Mock<ILogger<WebsiteService>>();
        var service = new WebsiteService(context, mockLogger.Object);
        var submission = new ContactSubmission("Test Name", "test@test.com", "Hello", "Message content", DateTime.UtcNow);

        var exception = await Record.ExceptionAsync(() => service.ProcessContactSubmissionAsync(submission));

        Assert.Null(exception); 
    }
}