using api.Adapters;
using api.Exceptions;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;


namespace tests;

[Collection("Database collection")]
public class TransportServiceShould
{
    private readonly DatabaseFixture fixture;

    public TransportServiceShould()
    {
        fixture = new DatabaseFixture();
    }

    [Fact]
    public async Task CreateNewTransport()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var transportService = new TransportService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedTransport = CreateTestTransport(project);

        // Act
        var projectResult = await transportService.CreateTransport(TransportDtoAdapter.Convert(expectedTransport), caseId);

        // Assert
        var actualTransport = projectResult.Transports.FirstOrDefault(o => o.Name == expectedTransport.Name);
        Assert.NotNull(actualTransport);
        TestHelper.CompareTransports(expectedTransport, actualTransport);
        var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
        Assert.Equal(actualTransport.Id, case_.TransportLink);
    }

    [Fact]
    public async Task ThrowNotInDatabaseExceptionWhenCreatingTransportWithBadProjectId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var transportService = new TransportService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedTransport = CreateTestTransport(new Project { Id = new Guid() });

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await transportService.CreateTransport(TransportDtoAdapter.Convert(expectedTransport), caseId));
    }

    [Fact]
    public async Task ThrowNotFoundInDatabaseExceptionWhenCreatingTransportWithBadCaseId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var transportService = new TransportService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var expectedTransport = CreateTestTransport(project);

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await transportService.CreateTransport(TransportDtoAdapter.Convert(expectedTransport), new Guid()));
    }

    [Fact]
    public async Task UpdateTransport()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var transportService = new TransportService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldTransport = CreateTestTransport(project);
        fixture.context.Transports.Add(oldTransport);
        fixture.context.SaveChanges();
        var updatedTransport = CreateUpdatedTransport(project, oldTransport);
        updatedTransport.Id = oldTransport.Id;

        // Act
        var projectResult = await transportService.UpdateTransport(TransportDtoAdapter.Convert(updatedTransport));

        // Assert
        var actualTransport = projectResult.Transports.FirstOrDefault(o => o.Name == updatedTransport.Name);
        Assert.NotNull(actualTransport);
        TestHelper.CompareTransports(updatedTransport, actualTransport);
    }

    private static Transport CreateUpdatedTransport(Project project, Transport oldTransport)
    {
        return new TransportBuilder
        {
            Id = oldTransport.Id,
            Name = "Updated Transport",
            Project = project,
            ProjectId = project.Id,
            GasExportPipelineLength = 100,
            OilExportPipelineLength = 100,
        }.WithCostProfile(new TransportCostProfile
        {
            Currency = Currency.USD,
            StartYear = 2030,
            Values = [23.4, 28.9, 24.3]
        })
            .WithTransportCessationCostProfile(new TransportCessationCostProfile
            {
                Currency = Currency.USD,
                StartYear = 2030,
                Values = [17.4, 17.9, 37.3]
            });
    }

    [Fact]
    public async Task ThrowArgumentExceptionIfTryingToUpdateNonExistentTransport()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var transportService = new TransportService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldTransport = CreateTestTransport(project);
        fixture.context.Transports.Add(oldTransport);
        fixture.context.SaveChanges();
        var updatedTransport = CreateUpdatedTransport(project, oldTransport);
        updatedTransport.Id = new Guid();

        // Act, assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await transportService.UpdateTransport(TransportDtoAdapter.Convert(updatedTransport)));
    }

    private static Transport CreateTestTransport(Project project)
    {
        return new TransportBuilder
        {
            Name = "Transport Transport",
            Project = project,
            ProjectId = project.Id,
            GasExportPipelineLength = 999,
            OilExportPipelineLength = 999,
        }.WithCostProfile(new TransportCostProfile
        {
            Currency = Currency.USD,
            StartYear = 2030,
            Values = [13.4, 18.9, 34.3]
        })
            .WithTransportCessationCostProfile(new TransportCessationCostProfile
            {
                Currency = Currency.USD,
                StartYear = 2030,
                Values = [13.4, 18.9, 34.3]
            });
    }
}
