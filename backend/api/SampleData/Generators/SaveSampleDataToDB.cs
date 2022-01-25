
using System.Collections.Generic;

using api.Context;
using api.SampleData.Builders;

namespace api.SampleData.Generators;

public static class SaveSampleDataToDB
{
    public static void PopulateDb(DcdDbContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        List<IProject> projects = new List<IProject>();
        projects.Add(new FirstProject());
        projects.Add(new SecondProject());
        projects.ForEach(p => context.AddRange(p.buildCases()));
        context.SaveChanges();
        // var firstProject = new FirstProject().build();
        // var secondProject = new SecondProject().build();
        List<ProjectBuilder> projectCases = new List<ProjectBuilder>();
        projectCases.Add(new FirstProjectCases(new FirstProject().getProject()).buildCases());
        context.AddRange(firstProject);
        context.AddRange(secondProject);
        firstProject = new FirstProjectCases().buildCases(firstProject);
        secondProject = SecondProjectCases.buildCases(secondProject);
        context.SaveChanges();
        //   projectsBuilder = SampleCaseGenerator.initializeCases(projectsBuilder);
        // foreach (ProjectBuilder p in projectsBuilder.Projects)
        // {
        //     context.AddRange(p.Cases);
        // }
        // context.SaveChanges();
    }
}
