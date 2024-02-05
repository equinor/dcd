using api.Adapters;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;

namespace tests;

[Collection("Database collection")]
public class SubstructureServiceShould : IDisposable
{
    private readonly DatabaseFixture fixture;
    public SubstructureServiceShould()
    {
        fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public async Task CreateNewSubstructure()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var substructureService = new SubstructureService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedSubstructure = CreateTestSubstructure(project);

        // Act
        var projectResult = await substructureService.CreateSubstructure(expectedSubstructure, caseId);

        // Assert
        var actualSubstructure = projectResult.Substructures.FirstOrDefault(o => o.Name == expectedSubstructure.Name);
        Assert.NotNull(actualSubstructure);
        TestHelper.CompareSubstructures(expectedSubstructure, actualSubstructure);
        var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
        Assert.Equal(actualSubstructure.Id, case_.SubstructureLink);
    }

    [Fact]
    public async Task ThrowNotInDatabaseExceptionWhenCreatingSubstructureWithBadProjectId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var substructureService = new SubstructureService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedSubstructure = CreateTestSubstructure(new Project { Id = new Guid() });

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await substructureService.CreateSubstructure(expectedSubstructure, caseId));
    }

    [Fact]
    public async Task ThrowNotFoundInDatabaseExceptionWhenCreatingSubstructureWithBadCaseId()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var substructureService = new SubstructureService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var expectedSubstructure = CreateTestSubstructure(project);

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await substructureService.CreateSubstructure(expectedSubstructure, new Guid()));
    }

    [Fact]
    public async Task DeleteSubstructure()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var substructureService = new SubstructureService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var substructureToDelete = CreateTestSubstructure(project);
        var sourceCaseId = project.Cases.FirstOrDefault().Id;
        await substructureService.CreateSubstructure(substructureToDelete, sourceCaseId);

        // Act
        var projectResult = await substructureService.DeleteSubstructure(substructureToDelete.Id);

        // Assert
        var actualSubstructure = projectResult.Substructures.FirstOrDefault(o => o.Name == substructureToDelete.Name);
        Assert.Null(actualSubstructure);
        var casesWithSubstructureLink = projectResult.Cases.Where(o => o.SubstructureLink == substructureToDelete.Id);
        Assert.Empty(casesWithSubstructureLink);
    }

    [Fact]
    public async Task ThrowArgumentExceptionIfTryingToDeleteNonExistentSubstructure()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var substructureService = new SubstructureService(fixture.context, projectService, loggerFactory);

        // Act, assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await substructureService.DeleteSubstructure(new Guid()));
    }

    [Fact]
    public async Task UpdateSubstructure()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var substructureService = new SubstructureService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldSubstructure = CreateTestSubstructure(project);
        fixture.context.Substructures.Add(oldSubstructure);
        fixture.context.SaveChanges();

        var updatedSubstructure = CreateUpdatedSubstructure(project, oldSubstructure);

        // Act
        var projectResult = await substructureService.UpdateSubstructure(SubstructureDtoAdapter.Convert(updatedSubstructure));

        // Assert
        var actualSubstructure = projectResult.Substructures.FirstOrDefault(o => o.Id == oldSubstructure.Id);
        Assert.NotNull(actualSubstructure);
        TestHelper.CompareSubstructures(updatedSubstructure, actualSubstructure);
    }

    [Fact]
    public async Task ThrowArgumentExceptionIfTryingToUpdateNonExistentSubstructure()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var substructureService = new SubstructureService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldSubstructure = CreateTestSubstructure(project);
        fixture.context.Substructures.Add(oldSubstructure);
        fixture.context.SaveChanges();
        var updatedSubstructure = CreateUpdatedSubstructure(project, oldSubstructure);
        updatedSubstructure.Id = new Guid();

        // Act, assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await substructureService.UpdateSubstructure(SubstructureDtoAdapter.Convert(updatedSubstructure)));
    }

    private static Substructure CreateTestSubstructure(Project project)
    {
        return new SubstructureBuilder
        {
            Project = project,
            ProjectId = project.Id,
            Name = "Substructure 11",
            Maturity = Maturity.A,
            DryWeight = 423.5,
        }
            .WithCostProfile(new SubstructureCostProfile
            {
                Currency = Currency.USD,
                StartYear = 1030,
                Values = new double[] { 23.4, 238.9, 32.3 }
            }
            );
    }

    private static Substructure CreateUpdatedSubstructure(Project project, Substructure oldSubstructure)
    {
        return new SubstructureBuilder
        {
            Id = oldSubstructure.Id,
            Project = project,
            ProjectId = project.Id,
            Name = "Substructure 1",
            Maturity = Maturity.B,
            DryWeight = 4.5,
        }
            .WithCostProfile(new SubstructureCostProfile
            {
                Currency = Currency.USD,
                StartYear = 2030,
                Values = new double[] { 23.4, 28.9, 32.3 }
            }
            );
    }
}
