using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using ProfileCore.Data;
using ProfileCore.Models.website;
using ProfileCore.Services;

namespace ProfileCore.Tests.Website;

public class WebsiteServiceTests
{
    private readonly Mock<ILogger<WebsiteService>> _mockLogger = new();
    private readonly Mock<IHostEnvironment> _mockEnv = new();

    private static WebsiteDbContext GetInMemoryDbContext()
    {
        return new WebsiteDbContext(new DbContextOptionsBuilder<WebsiteDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);
    }

    [Fact]
    public async Task GetProfileAsync_WhenProfileExists_ReturnsProfile()
    {
        using var context = GetInMemoryDbContext();
        context.Profile.Add(new Profile(1, "First", "Last", "Title", "Bio", null, ["C#"]));
        await context.SaveChangesAsync();

        var result = await new WebsiteService(context, _mockLogger.Object, _mockEnv.Object).GetProfileAsync();

        Assert.NotNull(result);
        Assert.Equal("First", result.FirstName);
    }

    [Fact]
    public async Task GetProfileAsync_WhenDatabaseIsEmpty_ThrowsInvalidOperationException()
    {
        using var context = GetInMemoryDbContext();

        await Assert.ThrowsAsync<InvalidOperationException>(() => new WebsiteService(context, _mockLogger.Object, _mockEnv.Object).GetProfileAsync());
    }

    [Fact]
    public async Task GetWorkExperienceAsync_ReturnsAllJobs()
    {
        using var context = GetInMemoryDbContext();
        
        context.Jobs.AddRange(
            new Job(1, null, "Role A", "Company A", [], "Jan 1", null, "Summary"),
            new Job(2, null, "Role B", "Company B", [], "Jan 1", null, "Summary")
        );
        await context.SaveChangesAsync();

        Assert.Equal(2, (await new WebsiteService(context, _mockLogger.Object, _mockEnv.Object).GetWorkExperienceAsync()).Count);
    }

    [Fact]
    public async Task GetSkillsAsync_AggregatesTechnologiesFromAllSources_WithoutDuplicates()
    {
        using var context = GetInMemoryDbContext();
        
        context.Jobs.Add(new Job(1, null, "Role", "Co", ["C#", "React"], "Jan 1", null, "Sum"));
        context.Projects.Add(new Project(1, null, "Proj", ["react", "SQL"], "Desc", "url", null, null )); // Note lower-case 'react'
        context.Courses.Add(new Course(1, "Math", "Desc", null, ["C#", "Python"]));
        await context.SaveChangesAsync();
        var result = await new WebsiteService(context, _mockLogger.Object, _mockEnv.Object).GetSkillsAsync();

        Assert.Equal(4, result.Count);
        var reactSkill = result.Single(s => s.SkillName.Equals("React", StringComparison.OrdinalIgnoreCase));
        Assert.Single(reactSkill.JobsSkillUsed);
        Assert.Single(reactSkill.ProjectsSkillUsed);
    }

    [Fact]
    public async Task GetSystemStatusAsync_ReturnsPopulatedStatusRecord()
    {
        using var context = GetInMemoryDbContext();
        
        _mockEnv.Setup(env => env.EnvironmentName).Returns("Testing");
        var result = await new WebsiteService(context, _mockLogger.Object, _mockEnv.Object).GetSystemStatusAsync();

        Assert.Equal("Testing", result.Environment);
        Assert.Equal("Operational", result.ApiStatus);
        Assert.Equal("Operational", result.DatabaseStatus); 
        Assert.True(result.UptimeMilliseconds > 0);
    }
}