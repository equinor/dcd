using System;
using System.Collections;
using System.Linq;

using api.Adapters;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using NuGet.Frameworks;

using Xunit;


namespace tests;

[Collection("Database collection")]
public class CaseShould : IDisposable
{
    private readonly DatabaseFixture fixture;
    private readonly IServiceProvider _serviceProvider;

    public CaseShould(DatabaseFixture fixture)
    {
        this.fixture = new DatabaseFixture();
        var serviceCollection = new ServiceCollection();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        fixture.Dispose();
    }

    [Fact]
    public void CreateNewCase()
    {
        var loggerFactory = new LoggerFactory();
        var project = fixture.context.Projects.FirstOrDefault();
        var actual = CreateCase(project);
        ProjectService projectService = new ProjectService(fixture.context, loggerFactory);
        CaseService caseService = new
            CaseService(fixture.context, projectService, loggerFactory, _serviceProvider);

        caseService.CreateCase(CaseDtoAdapter.Convert(actual));

        var cases = fixture.context.Projects.FirstOrDefault(o =>
                o.Name == project.Name).Cases;
        var expected = cases.FirstOrDefault(o => o.Name ==
                actual.Name);
        Assert.NotNull(expected);

        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.ReferenceCase, actual.ReferenceCase);
        Assert.Equal(expected.DG4Date, actual.DG4Date);
        Assert.Equal(expected.ModifyTime, actual.ModifyTime);
        Assert.Equal(expected.ReferenceCase, actual.ReferenceCase);
    }

    [Fact]
    public void UpdateCase()
    {
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var caseService = new CaseService(fixture.context, projectService, loggerFactory, _serviceProvider);
        var project = fixture.context.Projects.FirstOrDefault();
        var oldCase = CreateCase(project);
        fixture.context.Cases.Add(oldCase);
        fixture.context.SaveChanges();
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
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var caseService = new CaseService(fixture.context, projectService, loggerFactory, _serviceProvider);
        var project = fixture.context.Projects.FirstOrDefault();
        var caseItem = CreateCase(project);
        caseService.CreateCase(CaseDtoAdapter.Convert(caseItem));

        var cases = fixture.context.Projects.FirstOrDefault(o =>
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
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var caseService = new CaseService(fixture.context, projectService, loggerFactory, _serviceProvider);

        Assert.Throws<NotFoundInDBException>(() => caseService.DeleteCase(new Guid()));
    }

    [Fact]
    public void DuplicateCase()
    {
        var loggerFactory = new LoggerFactory();
        var projectService = new ProjectService(fixture.context, loggerFactory);
        var caseService = new CaseService(fixture.context, projectService, loggerFactory, _serviceProvider);

        var project = fixture.context.Projects.FirstOrDefault();
        var caseItem = CreateCase(project);
        caseService.CreateCase(CaseDtoAdapter.Convert(caseItem));

        var cases = fixture.context.Projects.FirstOrDefault(o =>
            o.Name == project.Name).Cases;
        var expected = cases.Where(o => o.Description ==
                caseItem.Description);
        Assert.True(expected.Count() == 1);

        caseService.DuplicateCase(expected.First().Id);
        Assert.True(expected.Count() == 2);
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
            DG4Date = DateTimeOffset.Now.AddDays(1)
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
            DG4Date = DateTimeOffset.Now.AddDays(1)
        };
    }
}
