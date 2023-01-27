using api.Adapters;
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
    public void UpdateCase()
    {
        var projectService = _caseServiceFixture.ProjectService;
        var caseService = _caseServiceFixture.CaseService;
        var project = _caseServiceFixture.DbContext.Projects.FirstOrDefault();
        var oldCase = CreateCase(project);
        _caseServiceFixture.DbContext.Cases.Add(oldCase);
        _caseServiceFixture.DbContext.SaveChanges();
        var updatedCase = CreateUpdatedCase(project, oldCase);

        // Act
        var projectResult = caseService.UpdateCase(CaseDtoAdapter.Convert(updatedCase));

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
    public void DeleteNonExistentCase()
    {
        var projectService = _caseServiceFixture.ProjectService;
        var caseService = _caseServiceFixture.CaseService;

        Assert.Throws<NotFoundInDBException>(() => caseService.DeleteCase(new Guid()));
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
