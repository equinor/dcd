using api.Adapters;
using api.Models;
using api.SampleData.Builders;
using api.SampleData.Generators;
using api.Services;

using Xunit;

namespace tests;

[Collection("Database collection")]
public class SurfServiceTest
{
    readonly DatabaseFixture fixture;

    public SurfServiceTest()
    {
        //arrange
        fixture = new DatabaseFixture();
    }

    public Surf InitializeTestSurf()
    {
        var project = fixture.context.Projects.FirstOrDefault();
        var testSurf = CreateTestSurf(project);
        fixture.context.Surfs.Add(testSurf);
        fixture.context.SaveChanges();
        return testSurf;
    }

    public SurfService GetSurfService()
    {
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var surfService = new SurfService(fixture.context, projectService, loggerFactory);
        return surfService;
    }

    [Fact]
    public async Task CreateNewSurf()
    {
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var surfService = new SurfService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(p => p.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedSurf = CreateTestSurf(project);

        // Act
        var projectResult = await surfService.CreateSurf(SurfDtoAdapter.Convert(expectedSurf), caseId);

        // Assert
        var actualSurf = SurfAdapter.Convert(projectResult.Surfs.FirstOrDefault(s => s.Name == expectedSurf.Name));
        Assert.NotNull(actualSurf);
        TestHelper.CompareSurfs(expectedSurf, actualSurf);
        var case_ = fixture.context.Cases.FirstOrDefault(c => c.Id == caseId);
        Assert.Equal(actualSurf.Id, case_.SurfLink);
    }

    [Fact]
    public async Task DeleteSurf()
    {
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault();
        var testSurf = InitializeTestSurf();
        fixture.context.Cases.Add(new Case
        {
            Project = project,
            SurfLink = testSurf.Id
        });
        fixture.context.SaveChanges();

        // Act
        var projectResult = await surfService.DeleteSurf(testSurf.Id);

        // Assert
        var actualSurf = projectResult.Surfs.FirstOrDefault(s => s.Name == testSurf.Name);
        Assert.Null(actualSurf);
        var casesWithSurfLink = projectResult.Cases.Where(c => c.SurfLink == testSurf.Id);
        Assert.Empty(casesWithSurfLink);
    }

    [Fact]
    public async Task UpdateSurf()
    {
        // Arrange
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault();
        var testSurf = InitializeTestSurf();
        var updatedSurf = CreateUpdatedSurf(project, testSurf);

        // Act
        var projectResult = await surfService.UpdateSurf(SurfDtoAdapter.Convert(updatedSurf));

        // Assert
        var actualSurf = SurfAdapter.Convert(projectResult.Surfs.FirstOrDefault(s => s.Name == updatedSurf.Name));
        Assert.NotNull(actualSurf);
        TestHelper.CompareSurfs(updatedSurf, actualSurf);
    }

    [Fact]
    public async Task ThrowNotInDatabaseExceptionWhenCreatingSurfWithBadProjectId()
    {
        // Arrange
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var testSurf = CreateTestSurf(new Project { Id = new Guid() });
        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await surfService.CreateSurf(SurfDtoAdapter.Convert(testSurf), caseId));
    }

    [Fact]
    public async Task ThrowNotFoundInDatabaseExceptionWhenCreatingSurfWithBadCaseId()
    {
        // Arrange
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var testSurf = CreateTestSurf(project);
        fixture.context.Cases.Add(new Case
        {
            Project = project,
            SurfLink = testSurf.Id
        });
        fixture.context.SaveChanges();

        // Act, assert
        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await surfService.CreateSurf(SurfDtoAdapter.Convert(testSurf), Guid.NewGuid()));
    }

    [Fact]
    public async Task ThrowArgumentExceptionIfTryingToUpdateNonExistentSurf()
    {
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault();
        var testSurf = InitializeTestSurf();

        var updatedSurf = SurfDtoAdapter.Convert(CreateUpdatedSurf(project, testSurf));

        // Act
        await surfService.DeleteSurf(updatedSurf.Id);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await surfService.UpdateSurf(updatedSurf));
    }

    [Fact]
    public async Task ThrowArgumentExceptionWhenTryingToDeleteNonExistentSurf()
    {
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault();
        var testSurf = InitializeTestSurf();

        // Act
        await surfService.DeleteSurf(testSurf.Id);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await surfService.DeleteSurf(testSurf.Id));
    }

    private static Surf CreateTestSurf(Project project)
    {
        return new SurfBuilder
        {
            Name = "Surf Test",
            Project = project,
            ProjectId = project.Id,
            ArtificialLift = ArtificialLift.GasLift,
            Currency = Currency.NOK,
            RiserCount = 10,
            TemplateCount = 11,
            InfieldPipelineSystemLength = 12,
            UmbilicalSystemLength = 13,
            Maturity = Maturity.D,
            ProductionFlowline = ProductionFlowline.HDPELinedCS,
        }
            .WithCostProfile(new SurfCostProfile
            {
                StartYear = 2030,
                Values = [2.3, 3.3, 4.4]
            }
            )
            .WithSurfCessationCostProfile(new SurfCessationCostProfile
            {
                StartYear = 2030,
                Values = [4.2, 5.2, 6.2]
            }
            );
    }

    private static Surf CreateUpdatedSurf(Project project, Surf oldSurf)
    {
        return new SurfBuilder
        {
            Id = oldSurf.Id,
            Name = "Updated surf",
            Project = project,
            ProjectId = project.Id,
            ArtificialLift = ArtificialLift.ElectricalSubmergedPumps,
            Currency = Currency.NOK,
            RiserCount = 11,
            TemplateCount = 12,
            InfieldPipelineSystemLength = 13,
            UmbilicalSystemLength = 14,
            Maturity = Maturity.B,
            ProductionFlowline = ProductionFlowline.SSClad_Insulation,
        }
            .WithCostProfile(new SurfCostProfile()
            {
                StartYear = 2031,
                Values = [5.5, 6.6, 7.7]
            }
            )
            .WithSurfCessationCostProfile(new SurfCessationCostProfile()
            {
                StartYear = 2032,
                Values = [7.7, 8.8, 9.9]
            }
            );
    }
}
