using api.Adapters;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;

namespace tests;

[Collection("Database collection")]
public class TopsideServiceShould : IDisposable
{
    private readonly DatabaseFixture fixture;

    public TopsideServiceShould()
    {
        fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public async Task CreateNewTopside()
    {
        // Arrange
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var testTopside = CreateTestTopside(project);
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var topsideService = new TopsideService(fixture.context, projectService, loggerFactory);

        // Act
        var projectResult = await topsideService.CreateTopside(TopsideDtoAdapter.Convert(testTopside), caseId);

        // Assert
        var retrievedTopside = projectResult.Topsides.FirstOrDefault(o => o.Name ==
                testTopside.Name);
        Assert.NotNull(retrievedTopside);
        TestHelper.CompareTopsides(testTopside, retrievedTopside);
        var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
        Assert.Equal(retrievedTopside.Id, case_.TopsideLink);
    }

    [Fact]
    public async Task ThrowNotInDatabaseExceptionWhenCreatingTopsideWithBadProjectId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var topsideService = new TopsideService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedTopside = CreateTestTopside(new Project { Id = new Guid() });

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await topsideService.CreateTopside(TopsideDtoAdapter.Convert(expectedTopside), caseId));
    }

    [Fact]
    public async Task ThrowNotFoundInDatabaseExceptionWhenCreatingTopsideWithBadCaseId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var topsideService = new TopsideService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var expectedTopside = CreateTestTopside(project);

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await topsideService.CreateTopside(TopsideDtoAdapter.Convert(expectedTopside), new Guid()));
    }

    [Fact]
    public async Task UpdateTopside()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var topsideService = new TopsideService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldTopside = CreateTestTopside(project);
        fixture.context.Topsides.Add(oldTopside);
        fixture.context.SaveChanges();
        var updatedTopside = CreateUpdatedTopside(project, oldTopside);

        // Act
        var projectResult = await topsideService.UpdateTopside(TopsideDtoAdapter.Convert(updatedTopside));

        // Assert
        var actualTopside = projectResult.Topsides.FirstOrDefault(o => o.Name == updatedTopside.Name);
        Assert.NotNull(actualTopside);
        TestHelper.CompareTopsides(updatedTopside, actualTopside);
    }

    [Fact]
    public async Task ThrowArgumentExceptionIfTryingToUpdateNonExistentTopside()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var topsideService = new TopsideService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldTopside = CreateTestTopside(project);
        fixture.context.Topsides.Add(oldTopside);
        fixture.context.SaveChanges();
        var updatedTopside = CreateUpdatedTopside(project, oldTopside);
        updatedTopside.Id = new Guid();

        // Act, assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await topsideService.UpdateTopside(TopsideDtoAdapter.Convert(updatedTopside)));
    }
    private static Topside CreateTestTopside(Project project)
    {
        return new TopsideBuilder
        {
            Name = "Test-topside-23",
            Project = project,
            ProjectId = project.Id,
            DryWeight = 0.3e6,
            OilCapacity = 50e6,
            GasCapacity = 0,
            ArtificialLift = ArtificialLift.GasLift,
            Maturity = Maturity.B
        }.WithCostProfile(new TopsideCostProfile
        {
            Currency = Currency.USD,
            StartYear = 2030,
            Values = [13.4, 18.9, 34.3]
        })
        .WithTopsideCessationCostProfile(new TopsideCessationCostProfile
        {
            Currency = Currency.NOK,
            StartYear = 2030,
            Values = [13.4, 183.9, 34.3]
        });
    }

    private static Topside CreateUpdatedTopside(Project project, Topside oldTopside)
    {
        return new TopsideBuilder
        {
            Id = oldTopside.Id,
            Name = "Test-topside-34",
            Project = project,
            ProjectId = project.Id,
            DryWeight = 5.3e6,
            OilCapacity = 52e6,
            GasCapacity = 7,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            Maturity = Maturity.C
        }.WithCostProfile(new TopsideCostProfile
        {
            Currency = Currency.NOK,
            StartYear = 2030,
            Values = [23.4, 283.9, 24.3]
        })
        .WithTopsideCessationCostProfile(new TopsideCessationCostProfile
        {
            Currency = Currency.NOK,
            StartYear = 2030,
            Values = [23.4, 283.9, 24.3]
        });
    }

}
