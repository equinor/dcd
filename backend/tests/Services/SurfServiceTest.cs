using System;
using System.Collections.Generic;
using System.Linq;

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
    readonly IOrderedEnumerable<api.SampleData.Builders.ProjectBuilder> projectsFromSampleDataGenerator;

    public SurfServiceTest(DatabaseFixture fixture)
    {
        //arrange
        this.fixture = new DatabaseFixture();
        projectsFromSampleDataGenerator = SampleCaseGenerator.initializeCases(SampleAssetGenerator.initializeAssets()).Projects.OrderBy(p => p.Name);
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
    public void GetAllSurf()
    {
        var loggerFactory = new LoggerFactory();
        ProjectService projectService = new ProjectService(fixture.context, loggerFactory);
        SurfService surfService = new SurfService(fixture.context, projectService, loggerFactory);
        var project = projectsFromSampleDataGenerator.First();
        surfService.GetSurfs(project.Id);
    }

    [Fact]
    public void CreateNewSurf()
    {
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var surfService = new SurfService(fixture.context, projectService, loggerFactory);
        var project = fixture.context.Projects.FirstOrDefault(p => p.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var expectedSurf = CreateTestSurf(project);

        // Act
        var projectResult = surfService.CreateSurf(SurfDtoAdapter.Convert(expectedSurf), caseId);

        // Assert
        var actualSurf = SurfAdapter.Convert(projectResult.Surfs.FirstOrDefault(s => s.Name == expectedSurf.Name));
        Assert.NotNull(actualSurf);
        TestHelper.CompareSurfs(expectedSurf, actualSurf);
        var case_ = fixture.context.Cases.FirstOrDefault(c => c.Id == caseId);
        Assert.Equal(actualSurf.Id, case_.SurfLink);
    }

    [Fact]
    public void DeleteSurf()
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
        var projectResult = surfService.DeleteSurf(testSurf.Id);

        // Assert
        var actualSurf = projectResult.Surfs.FirstOrDefault(s => s.Name == testSurf.Name);
        Assert.Null(actualSurf);
        var casesWithSurfLink = projectResult.Cases.Where(c => c.SurfLink == testSurf.Id);
        Assert.Empty(casesWithSurfLink);
    }

    [Fact]
    public void UpdateSurf()
    {
        // Arrange
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault();
        var testSurf = InitializeTestSurf();
        var updatedSurf = CreateUpdatedSurf(project, testSurf);

        // Act
        var projectResult = surfService.UpdateSurf(SurfDtoAdapter.Convert(updatedSurf));

        // Assert
        var actualSurf = SurfAdapter.Convert(projectResult.Surfs.FirstOrDefault(s => s.Name == updatedSurf.Name));
        Assert.NotNull(actualSurf);
        TestHelper.CompareSurfs(updatedSurf, actualSurf);
    }

    [Fact]
    public void ThrowNotInDatabaseExceptionWhenCreatingSurfWithBadProjectId()
    {
        // Arrange
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
        var caseId = project.Cases.FirstOrDefault().Id;
        var testSurf = CreateTestSurf(new Project { Id = new Guid() });
        // Act, assert
        Assert.Throws<NotFoundInDBException>(() => surfService.CreateSurf(SurfDtoAdapter.Convert(testSurf), caseId));
    }

    [Fact]
    public void ThrowNotFoundInDatabaseExceptionWhenCreatingSurfWithBadCaseId()
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
        Assert.Throws<NotFoundInDBException>(() => surfService.CreateSurf(SurfDtoAdapter.Convert(testSurf), Guid.NewGuid()));
    }

    [Fact]
    public void ThrowArgumentExceptionIfTryingToUpdateNonExistentSurf()
    {
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault();
        var testSurf = InitializeTestSurf();

        var updatedSurf = SurfDtoAdapter.Convert(CreateUpdatedSurf(project, testSurf));

        // Act
        surfService.DeleteSurf(updatedSurf.Id);

        // Assert
        Assert.Throws<ArgumentException>(() => surfService.UpdateSurf(updatedSurf));
    }

    [Fact]
    public void ThrowArgumentExceptionWhenTryingToDeleteNonExistentSurf()
    {
        var surfService = GetSurfService();
        var project = fixture.context.Projects.FirstOrDefault();
        var testSurf = InitializeTestSurf();

        // Act
        surfService.DeleteSurf(testSurf.Id);

        // Assert
        Assert.Throws<ArgumentException>(() => surfService.DeleteSurf(testSurf.Id));
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
                Values = new double[] { 2.3, 3.3, 4.4 }
            }
            )
            .WithSurfCessationCostProfile(new SurfCessationCostProfile
            {
                StartYear = 2030,
                Values = new double[] { 4.2, 5.2, 6.2 }
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
                Values = new double[] { 5.5, 6.6, 7.7 }
            }
            )
            .WithSurfCessationCostProfile(new SurfCessationCostProfile()
            {
                StartYear = 2032,
                Values = new double[] { 7.7, 8.8, 9.9 }
            }
            );
    }
}
