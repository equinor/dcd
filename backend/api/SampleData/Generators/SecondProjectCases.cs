using api.Models;
using api.SampleData.Builders;

namespace api.SampleData.Generators;
public class SecondProjectCases : IProjectCases
{
    public ProjectBuilder addCases(ProjectBuilder projectBuilder)
    {
        string project2DrainageStrategyName1 = projectBuilder.DrainageStrategies.ToList()[0].Name;
        projectBuilder
        .WithCase(new CaseBuilder()
        {
            Name = "Case 1 in P2",
            Description = "Description Case 1 P2"
        }
            .WithDrainageStrategy(project2DrainageStrategyName1, projectBuilder)
        );
        return projectBuilder;
    }
}
