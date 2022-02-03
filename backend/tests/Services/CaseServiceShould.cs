using System;
using System.Linq;

using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;


namespace tests;

[Collection("Database collection")]
public class CaseShould : IDisposable
{
    private readonly DatabaseFixture fixture;

    public CaseShould(DatabaseFixture fixture)
    {
        this.fixture = new DatabaseFixture();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public void CreateNewCase()
    {
        var project = fixture.context.Projects.FirstOrDefault();
        var actual = CreateCase(project);
        ProjectService projectService = new ProjectService(fixture.context);
        CaseService caseService = new
            CaseService(fixture.context, projectService);

        caseService.CreateCase(actual);

        var cases = fixture.context.Projects.FirstOrDefault(o =>
                o.Name == project.Name).Cases;
        var expected = cases.FirstOrDefault(o => o.Name ==
                actual.Name);
        Assert.NotNull(expected);

        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.ReferenceCase, actual.ReferenceCase);
        Assert.Equal(expected.DG4Date, actual.DG4Date);
        Assert.Equal(expected.CreateTime, actual.CreateTime);
        Assert.Equal(expected.ModifyTime, actual.ModifyTime);
        Assert.Equal(expected.ReferenceCase, actual.ReferenceCase);

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
            DG4Date = DateTimeOffset.Now.AddDays(1)
        };

    }

}
