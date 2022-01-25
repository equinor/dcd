// using api.Models;
// using api.SampleData.Builders;

// namespace api.SampleData.Generators;

// public static class SampleCaseGenerator
// {

//     public static ProjectsBuilder initializeCases(ProjectsBuilder projectsBuilder)
//     {
//         const string project1 = "P1";
//         string project1DrainageStrategyName1 = projectsBuilder.ForProject(project1).DrainageStrategies.ToList()[0].Name;
//         string project1DrainageStrategyName2 = projectsBuilder.ForProject(project1).DrainageStrategies.ToList()[1].Name;
//         string wellProjectName = projectsBuilder.ForProject(project1).WellProjects.ToList()[0].Name;
//         string project1SurfName = projectsBuilder.ForProject(project1).Surfs.ToList()[0].Name;
//         string project1SubstructureName = projectsBuilder.ForProject(project1).Substructures.ToList()[0].Name;
//         string project1TopsideName = projectsBuilder.ForProject(project1).Topsides.ToList()[0].Name;
//         string project1TransportName = projectsBuilder.ForProject(project1).Transports.ToList()[0].Name;
//         projectsBuilder.ForProject(project1)
//         .WithCase(new CaseBuilder()
//         {
//             Name = "Case 1 in P1",
//             Description = "Description Case 1 in P1",
//             CreateTime = DateTimeOffset.UtcNow,
//             ModifyTime = DateTimeOffset.UtcNow,
//             ReferenceCase = true,
//             ProducerCount = 2,
//             GasInjectorCount = 3,
//             WaterInjectorCount = 4,
//             RiserCount = 5,
//             TemplateCount = 6,
//             FacilitiesAvailability = 0.8,
//             DG4Date = DateTimeOffset.Now.AddYears(5),
//             ArtificialLift = ArtificialLift.GasLift
//         }
//             .WithCessationCost(new CessationCostBuilder()
//             {
//                 Currency = api.Models.Currency.NOK
//             }
//                 .WithYearValue(2030, 1000)
//                 .WithYearValue(2031, 1100)
//                 .WithYearValue(2032, 1200)
//             )
//             .WithDrainageStrategy(project1DrainageStrategyName1, projectsBuilder.ForProject(project1))
//             .WithWellProject(wellProjectName, projectsBuilder.ForProject(project1))
//             .WithSurf(project1SurfName, projectsBuilder.ForProject(project1))
//             .WithTopside(project1TopsideName, projectsBuilder.ForProject(project1))
//             )
//         .WithCase(new CaseBuilder()
//         {
//             Name = "Case 2 in P1",
//             Description = "Description 2 in Case 2 in P1"
//         }
//             .WithDrainageStrategy(project1DrainageStrategyName1, projectsBuilder.ForProject(project1))
//             .WithSubstructure(project1SubstructureName, projectsBuilder.ForProject(project1))
//             .WithTransport(project1TransportName, projectsBuilder.ForProject(project1))
//         )
//         .WithCase(new CaseBuilder()
//         {
//             Name = "Case 3 in P1",
//             Description = "Description 2 in Case 1 in P1"
//         });

//         const string project2 = "P2";
//         string project2DrainageStrategyName1 = projectsBuilder.ForProject(project2).DrainageStrategies.ToList()[0].Name;
//         projectsBuilder.ForProject(project2)
//         .WithCase(new CaseBuilder()
//         {
//             Name = "Case 1 in P2",
//             Description = "Description Case 1 P2"
//         }
//             .WithDrainageStrategy(project2DrainageStrategyName1, projectsBuilder.ForProject(project2))
//         );
//         return projectsBuilder;
//     }
// }
