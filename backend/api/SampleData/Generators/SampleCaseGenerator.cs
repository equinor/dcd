using api.SampleData.Builders;

namespace api.SampleData.Generators;

public static class SampleCaseGenerator
{
    public static ProjectsBuilder initializeCases(ProjectsBuilder projectsBuilder)
    {
        const string projectSkarven = "Skarven";
        const string drainStratSkarven = "SkarvenDrainStrat";
        projectsBuilder.ForProject(projectSkarven)
            .WithCase(new CaseBuilder
                {
                    Name = "BusCase 1",
                    Description = "Desc Bus Case 1",
                    DG4Date = DateTimeOffset.Now,
                    Id = new Guid("43666744-b4be-4cbb-b04f-e0831a87eb85")
                }
                .WithDrainageStrategy(drainStratSkarven, projectsBuilder.ForProject(projectSkarven))
                .WithWellProject("SkarvenWell", projectsBuilder.ForProject(projectSkarven))
                .WithSubstructure("SkarvenSub", projectsBuilder.ForProject(projectSkarven))
                .WithExploration("SkarvenExpl", projectsBuilder.ForProject(projectSkarven))
            ).WithCase(SampleAssetGenerator.case2Case()
                .WithDrainageStrategy("SkarvenDrainStratCase2", projectsBuilder.ForProject(projectSkarven))
                .WithWellProject("SkarvenWellCase2", projectsBuilder.ForProject(projectSkarven))
                .WithSubstructure("SkarvenSubCase2", projectsBuilder.ForProject(projectSkarven))
                .WithExploration("SkarvenExplCase2", projectsBuilder.ForProject(projectSkarven))
            );

        const string project1 = "P1";
        var project1DrainageStrategyName1 = projectsBuilder.ForProject(project1).DrainageStrategies!.ToList()[0].Name;
        var wellProjectName = projectsBuilder.ForProject(project1).WellProjects!.ToList()[0].Name;
        var project1SurfName = projectsBuilder.ForProject(project1).Surfs!.ToList()[0].Name;
        var project1SubstructureName = projectsBuilder.ForProject(project1).Substructures!.ToList()[0].Name;
        var project1TopsideName = projectsBuilder.ForProject(project1).Topsides!.ToList()[0].Name;
        var project1TransportName = projectsBuilder.ForProject(project1).Transports!.ToList()[0].Name;
        var projectExplorationName = projectsBuilder.ForProject(project1).Explorations!.ToList()[0].Name;
        projectsBuilder.ForProject(project1)
            .WithCase(new CaseBuilder
                {
                    Name = "Case 1 in P1",
                    Description = "Description Case 1 in P1",
                    CreateTime = DateTimeOffset.UtcNow,
                    ModifyTime = DateTimeOffset.UtcNow,
                    ReferenceCase = true,
                    DG4Date = DateTimeOffset.Now.AddYears(5),
                    Id = new Guid("afe4448b-0773-4553-b57b-d909ebf9c79d")
                }
                .WithDrainageStrategy(project1DrainageStrategyName1, projectsBuilder.ForProject(project1))
                .WithWellProject(wellProjectName, projectsBuilder.ForProject(project1))
                .WithSurf(project1SurfName, projectsBuilder.ForProject(project1))
                .WithTopside(project1TopsideName, projectsBuilder.ForProject(project1))
                .WithTransport(project1TransportName, projectsBuilder.ForProject(project1))
                .WithSubstructure(project1SubstructureName, projectsBuilder.ForProject(project1))
                .WithExploration(projectExplorationName, projectsBuilder.ForProject(project1))
            )
            .WithCase(new CaseBuilder
                {
                    Name = "Case 2 in P1",
                    Description = "Description 2 in Case 2 in P1",
                    DG4Date = DateTimeOffset.Now
                }
                .WithDrainageStrategy(project1DrainageStrategyName1, projectsBuilder.ForProject(project1))
                .WithSubstructure(project1SubstructureName, projectsBuilder.ForProject(project1))
                .WithTransport(project1TransportName, projectsBuilder.ForProject(project1))
            )
            .WithCase(new CaseBuilder
            {
                Name = "Case 3 in P1",
                Description = "Description 2 in Case 1 in P1",
                DG4Date = DateTimeOffset.Now
            });

        const string project2 = "P2";
        var project2DrainageStrategyName1 = projectsBuilder.ForProject(project2).DrainageStrategies!.ToList()[0].Name;
        projectsBuilder.ForProject(project2)
            .WithCase(new CaseBuilder
                {
                    Name = "Case 1 in P2",
                    Description = "Description Case 1 P2",
                    DG4Date = DateTimeOffset.Now,
                    Id = new Guid("f3b58556-68f9-4730-a378-c153e2f0cfa6")
                }
                .WithDrainageStrategy(project2DrainageStrategyName1, projectsBuilder.ForProject(project2))
            );
        return projectsBuilder;
    }
}
