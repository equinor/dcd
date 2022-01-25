using api.Models;
using api.SampleData.Builders;

namespace api.SampleData.Generators;
public class FirstProjectCases : IProject
{
    ProjectBuilder projectBuilder;
    public FirstProjectCases(ProjectBuilder projectBuilder)
    {
        this.projectBuilder = projectBuilder;
    }

    public ProjectBuilder buildCases()
    {
        this.projectBuilder = addCases(this.projectBuilder);
        return projectBuilder;
    }
    private ProjectBuilder addCases(ProjectBuilder projectBuilder)
    {

        string project1DrainageStrategyName1 = projectBuilder.DrainageStrategies.ToList()[0].Name;
        string project1DrainageStrategyName2 = projectBuilder.DrainageStrategies.ToList()[1].Name;
        string wellProjectName = projectBuilder.WellProjects.ToList()[0].Name;
        string project1SurfName = projectBuilder.Surfs.ToList()[0].Name;
        string project1SubstructureName = projectBuilder.Substructures.ToList()[0].Name;
        string project1TopsideName = projectBuilder.Topsides.ToList()[0].Name;
        string project1TransportName = projectBuilder.Transports.ToList()[0].Name;
        projectBuilder
        .WithCase(new CaseBuilder()
        {
            Name = "Case 1 in P1",
            Description = "Description Case 1 in P1",
            CreateTime = DateTimeOffset.UtcNow,
            ModifyTime = DateTimeOffset.UtcNow,
            ReferenceCase = true,
            ProducerCount = 2,
            GasInjectorCount = 3,
            WaterInjectorCount = 4,
            RiserCount = 5,
            TemplateCount = 6,
            FacilitiesAvailability = 0.8,
            DG4Date = DateTimeOffset.Now.AddYears(5),
            ArtificialLift = ArtificialLift.GasLift
        }
            .WithCessationCost(new CessationCostBuilder()
            {
                Currency = api.Models.Currency.NOK
            }
                .WithYearValue(2030, 1000)
                .WithYearValue(2031, 1100)
                .WithYearValue(2032, 1200)
            )
            .WithDrainageStrategy(project1DrainageStrategyName1, projectBuilder)
            .WithWellProject(wellProjectName, projectBuilder)
            .WithSurf(project1SurfName, projectBuilder)
            .WithTopside(project1TopsideName, projectBuilder)
            )
        .WithCase(new CaseBuilder()
        {
            Name = "Case 2 in P1",
            Description = "Description 2 in Case 2 in P1"
        }
            .WithDrainageStrategy(project1DrainageStrategyName1, projectBuilder)
            .WithSubstructure(project1SubstructureName, projectBuilder)
            .WithTransport(project1TransportName, projectBuilder)
        )
        .WithCase(new CaseBuilder()
        {
            Name = "Case 3 in P1",
            Description = "Description 2 in Case 1 in P1"
        });
        return projectBuilder;
    }
}