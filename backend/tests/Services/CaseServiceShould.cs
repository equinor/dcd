using api.Adapters;
using api.Dtos;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using tests.Fixtures;

using Xunit;

namespace tests;

[Collection("Database collection")]
public class CaseShould : IClassFixture<CaseServiceFixture>
{
    private readonly CaseServiceFixture _caseServiceFixture;

    public CaseShould(CaseServiceFixture caseServiceFixture)
    {
        _caseServiceFixture = caseServiceFixture;
    }

    [Fact]
    public void CreateNewCase()
    {
        var project = _caseServiceFixture.DbContext.Projects.FirstOrDefault();
        var actual = CreateCase(project);
        var projectService = _caseServiceFixture.ProjectService;
        var caseService = _caseServiceFixture.CaseService;

        caseService.CreateCase(CaseDtoAdapter.Convert(actual));

        var cases = _caseServiceFixture.DbContext.Projects.FirstOrDefault(o =>
                o.Name == project.Name).Cases;
        var expected = cases.FirstOrDefault(o => o.Name ==
                actual.Name);
        Assert.NotNull(expected);

        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.ReferenceCase, actual.ReferenceCase);
        Assert.Equal(expected.DG4Date, actual.DG4Date);
        Assert.Equal(expected.ReferenceCase, actual.ReferenceCase);
    }

    [Fact]
    public void NewCreateCase_CreateCaseFromMinimalDto_CaseCreated()
    {
        // Arrange
        var projectId = _caseServiceFixture.DbContext.Projects.First().Id;

        var dto = new CaseDto
        {
            Name = "NewCreateCase_CreateCaseFromMinimalDto_CaseCreated",
            Description = "Description for NewCreateCase_CreateCaseFromMinimalDto_CaseCreated",
            ProjectId = projectId,
        };

        var caseService = _caseServiceFixture.CaseService;
        var dbContext = _caseServiceFixture.DbContext;

        // Act
        caseService.NewCreateCase(dto);

        var createdCase = dbContext.Cases.FirstOrDefault(c => c.Name == "NewCreateCase_CreateCaseFromMinimalDto_CaseCreated");

        // Assert
        Assert.NotNull(createdCase);

        Assert.Equal(dto.Name, createdCase.Name);
        Assert.Equal(dto.Description, createdCase.Description);
        Assert.Equal(dto.ProjectId, createdCase.ProjectId);

        Assert.NotEqual(createdCase.SurfLink, Guid.Empty);
        Assert.NotEqual(createdCase.SubstructureLink, Guid.Empty);
        Assert.NotEqual(createdCase.TopsideLink, Guid.Empty);
        Assert.NotEqual(createdCase.TransportLink, Guid.Empty);
        Assert.NotEqual(createdCase.DrainageStrategyLink, Guid.Empty);
        Assert.NotEqual(createdCase.WellProjectLink, Guid.Empty);
        Assert.NotEqual(createdCase.ExplorationLink, Guid.Empty);
    }

    [Fact]
    public void NewCreateCase_CreateCaseFromDto_CaseCreated()
    {
        // Arrange
        var projectId = _caseServiceFixture.DbContext.Projects.First().Id;

        var dto = new CaseDto
        {
            ProjectId = projectId,
            Name = "NewCreateCase_CreateCaseFromDto_CaseCreated",
            Description = "Description for NewCreateCase_CreateCaseFromDto_CaseCreated",
            ArtificialLift = ArtificialLift.SubseaBoosterPumps,
            ProductionStrategyOverview = ProductionStrategyOverview.WAG,
            ProducerCount = 2,
            GasInjectorCount = 3,
            WaterInjectorCount = 4,
            FacilitiesAvailability = 0.75,
            NPV = 0.5,
            BreakEven = 2,
            Host = "Host",

            DGADate = new DateTime(2007, 07, 12, 06, 32, 00),
            DGBDate = new DateTime(2007, 07, 12, 06, 32, 00),
            DGCDate = new DateTime(2007, 07, 12, 06, 32, 00),
            APXDate = new DateTime(2007, 07, 12, 06, 32, 00),
            APZDate = new DateTime(2007, 07, 12, 06, 32, 00),
            DG0Date = new DateTime(2007, 07, 12, 06, 32, 00),
            DG1Date = new DateTime(2007, 07, 12, 06, 32, 00),
            DG2Date = new DateTime(2007, 07, 12, 06, 32, 00),
            DG3Date = new DateTime(2007, 07, 12, 06, 32, 00),
            DG4Date = new DateTime(2007, 07, 12, 06, 32, 00),
        };

        var caseService = _caseServiceFixture.CaseService;
        var dbContext = _caseServiceFixture.DbContext;

        // Act
        caseService.NewCreateCase(dto);

        var createdCase = dbContext.Cases.FirstOrDefault(c => c.Name == "NewCreateCase_CreateCaseFromDto_CaseCreated");

        // Assert
        Assert.NotNull(createdCase);

        Assert.Equal(dto.ProjectId, createdCase.ProjectId);
        Assert.Equal(dto.Name, createdCase.Name);
        Assert.Equal(dto.Description, createdCase.Description);

        Assert.Equal(dto.ArtificialLift, createdCase.ArtificialLift);
        Assert.Equal(dto.ProductionStrategyOverview, createdCase.ProductionStrategyOverview);
        Assert.Equal(dto.ProducerCount, createdCase.ProducerCount);
        Assert.Equal(dto.GasInjectorCount, createdCase.GasInjectorCount);
        Assert.Equal(dto.WaterInjectorCount, createdCase.WaterInjectorCount);
        Assert.Equal(dto.FacilitiesAvailability, createdCase.FacilitiesAvailability);
        Assert.True(createdCase.CapexFactorFeasibilityStudies == 0.015);
        Assert.True(createdCase.CapexFactorFEEDStudies == 0.015);
        Assert.Equal(dto.NPV, createdCase.NPV);
        Assert.Equal(dto.BreakEven, createdCase.BreakEven);
        Assert.Equal(dto.Host, createdCase.Host);

        Assert.Equal(dto.DGADate, createdCase.DGADate);
        Assert.Equal(dto.DGBDate, createdCase.DGBDate);
        Assert.Equal(dto.DGCDate, createdCase.DGCDate);
        Assert.Equal(dto.APXDate, createdCase.APXDate);
        Assert.Equal(dto.APZDate, createdCase.APZDate);
        Assert.Equal(dto.DG0Date, createdCase.DG0Date);
        Assert.Equal(dto.DG1Date, createdCase.DG1Date);
        Assert.Equal(dto.DG2Date, createdCase.DG2Date);
        Assert.Equal(dto.DG3Date, createdCase.DG3Date);
        Assert.Equal(dto.DG4Date, createdCase.DG4Date);

        Assert.NotEqual(DateTimeOffset.MinValue, createdCase.CreateTime);
        Assert.NotEqual(DateTimeOffset.MinValue, createdCase.ModifyTime);

        Assert.NotEqual(createdCase.SurfLink, Guid.Empty);
        Assert.NotEqual(createdCase.SubstructureLink, Guid.Empty);
        Assert.NotEqual(createdCase.TopsideLink, Guid.Empty);
        Assert.NotEqual(createdCase.TransportLink, Guid.Empty);
        Assert.NotEqual(createdCase.DrainageStrategyLink, Guid.Empty);
        Assert.NotEqual(createdCase.WellProjectLink, Guid.Empty);
        Assert.NotEqual(createdCase.ExplorationLink, Guid.Empty);
    }

    [Fact]
    public async Task UpdateCase()
    {
        var projectService = _caseServiceFixture.ProjectService;
        var caseService = _caseServiceFixture.CaseService;
        var project = _caseServiceFixture.DbContext.Projects.FirstOrDefault();
        var oldCase = CreateCase(project);
        _caseServiceFixture.DbContext.Cases.Add(oldCase);
        _caseServiceFixture.DbContext.SaveChanges();
        var updatedCase = CreateUpdatedCase(project, oldCase);

        // Act
        var projectResult = await caseService.UpdateCase(CaseDtoAdapter.Convert(updatedCase));

        // Assert
        var actualCase = projectResult.Cases.FirstOrDefault(o => o.Name == updatedCase.Name);
        Assert.NotNull(actualCase);
        TestHelper.CompareCases(updatedCase, actualCase);
    }

    [Fact]
    public void DeleteCase()
    {
        var projectService = _caseServiceFixture.ProjectService;
        var caseService = _caseServiceFixture.CaseService;
        var project = _caseServiceFixture.DbContext.Projects.FirstOrDefault();
        var caseItem = CreateCase(project);
        caseItem.Name = "case to be deleted";
        caseService.CreateCase(CaseDtoAdapter.Convert(caseItem));

        var cases = _caseServiceFixture.DbContext.Projects.FirstOrDefault(o =>
        o.Name == project.Name).Cases;
        var expected = cases.FirstOrDefault(o => o.Name ==
                caseItem.Name);
        Assert.NotNull(expected);

        caseService.DeleteCase(expected.Id);
        var deleted = cases.FirstOrDefault(o => o.Name ==
                caseItem.Name);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteNonExistentCase()
    {
        var projectService = _caseServiceFixture.ProjectService;
        var caseService = _caseServiceFixture.CaseService;

        await Assert.ThrowsAsync<NotFoundInDBException>(async () => await caseService.DeleteCase(new Guid()));
    }

    private static Case CreateUpdatedCase(Project project, Case oldCase)
    {
        return new CaseBuilder
        {
            Id = oldCase.Id,
            ProjectId = project.Id,
            Name = "Test-exploration-34",
            Project = project,
            Description = "descUpdated",
            ReferenceCase = false,
            DG4Date = DateTimeOffset.UtcNow.AddDays(1)
        };
    }

    private static Case CreateCase(Project project)
    {
        return new CaseBuilder
        {
            ProjectId = project.Id,
            Name = "Test-exploration-23",
            Project = project,
            Description = "desc",
            ReferenceCase = false,
            DG4Date = DateTimeOffset.UtcNow.AddDays(1)
        };
    }
}
