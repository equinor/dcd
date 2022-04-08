using System;
using System.Linq;

using api.Adapters;
using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;


namespace tests
{
    [Collection("Database collection")]
    public class WellProjectServiceShould : IDisposable
    {
        private readonly DatabaseFixture fixture;

        public WellProjectServiceShould()
        {
            fixture = new DatabaseFixture();
        }

        public void Dispose()
        {
            fixture.Dispose();
        }

        [Fact]
        public void GetWellProjects()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var wellProjectService = new WellProjectService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedWellProjects = fixture.context.WellProjects.ToList().Where(o => o.Project.Id == project.Id);

            // Act
            var actualWellProjects = wellProjectService.GetWellProjects(project.Id);

            // Assert
            Assert.Equal(expectedWellProjects.Count(), actualWellProjects.Count());
            var wellProjectsExpectedAndActual = expectedWellProjects.OrderBy(d => d.Name)
                .Zip(actualWellProjects.OrderBy(d => d.Name));
            foreach (var wellProjectPair in wellProjectsExpectedAndActual)
            {
                TestHelper.CompareWellProjects(wellProjectPair.First, wellProjectPair.Second);
            }
        }

        [Fact]
        public void CreateNewWellProject()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var wellProjectService = new WellProjectService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
            var caseId = project.Cases.FirstOrDefault().Id;
            var expectedWellProject = CreateTestWellProject(project);

            // Act
            var projectResult = wellProjectService.CreateWellProject(expectedWellProject, caseId);

            // Assert
            var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == expectedWellProject.Name);
            Assert.NotNull(actualWellProject);
            TestHelper.CompareWellProjects(expectedWellProject, actualWellProject);
            var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
            Assert.Equal(actualWellProject.Id, case_.WellProjectLink);
        }

        [Fact]
        public void ThrowNotInDatabaseExceptionWhenCreatingWellProjectWithBadProjectId()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var wellProjectService = new WellProjectService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
            var caseId = project.Cases.FirstOrDefault().Id;
            var expectedWellProject = CreateTestWellProject(new Project { Id = new Guid() });

            // Act, assert
            Assert.Throws<NotFoundInDBException>(() => wellProjectService.CreateWellProject(expectedWellProject, caseId));
        }

        [Fact]
        public void ThrowNotFoundInDatabaseExceptionWhenCreatingWellProjectWithBadCaseId()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var wellProjectService = new WellProjectService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
            var expectedWellProject = CreateTestWellProject(project);

            // Act, assert
            Assert.Throws<NotFoundInDBException>(() => wellProjectService.CreateWellProject(expectedWellProject, new Guid()));
        }

        [Fact]
        public void DeleteWellProject()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var wellProjectService = new WellProjectService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var wellProjectToDelete = CreateTestWellProject(project);
            fixture.context.WellProjects.Add(wellProjectToDelete);
            fixture.context.Cases.Add(new Case
            {
                Project = project,
                WellProjectLink = wellProjectToDelete.Id
            });
            fixture.context.SaveChanges();

            // Act
            var projectResult = wellProjectService.DeleteWellProject(wellProjectToDelete.Id);

            // Assert
            var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == wellProjectToDelete.Name);
            Assert.Null(actualWellProject);
            var casesWithWellProjectLink = projectResult.Cases.Where(o => o.WellProjectLink == wellProjectToDelete.Id);
            Assert.Empty(casesWithWellProjectLink);
        }

        [Fact]
        public void ThrowArgumentExceptionIfTryingToDeleteNonExistentWellProject()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var wellProjectService = new WellProjectService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var wellProjectToDelete = CreateTestWellProject(project);
            fixture.context.WellProjects.Add(wellProjectToDelete);
            fixture.context.SaveChanges();

            // Act, assert
            Assert.Throws<ArgumentException>(() => wellProjectService.DeleteWellProject(new Guid()));
        }

        [Fact]
        public void UpdateWellProject()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var wellProjectService = new WellProjectService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var oldWellProject = CreateTestWellProject(project);
            fixture.context.WellProjects.Add(oldWellProject);
            fixture.context.SaveChanges();
            var updatedWellProject = CreateUpdatedWellProject(project);
            updatedWellProject.Id = oldWellProject.Id;

            // Act
            var projectResult = wellProjectService.UpdateWellProject(WellProjectDtoAdapter.Convert(updatedWellProject));

            // Assert
            var actualWellProject = projectResult.WellProjects.FirstOrDefault(o => o.Name == updatedWellProject.Name);
            Assert.NotNull(actualWellProject);
            TestHelper.CompareWellProjects(updatedWellProject, actualWellProject);
        }

        [Fact]
        public void ThrowArgumentExceptionIfTryingToUpdateNonExistentWellProject()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var wellProjectService = new WellProjectService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var oldWellProject = CreateTestWellProject(project);
            fixture.context.WellProjects.Add(oldWellProject);
            fixture.context.SaveChanges();
            var updatedWellProject = CreateUpdatedWellProject(project);

            // Act, assert
            // Assert.Throws<ArgumentException>(() => wellProjectService.UpdateWellProject(updatedWellProject));
        }

        private static WellProject CreateTestWellProject(Project project)
        {
            return new WellProjectBuilder
            {
                Name = "DrainStrat Test",
                ProducerCount = 3,
                GasInjectorCount = 4,
                WaterInjectorCount = 5,
                ArtificialLift = ArtificialLift.ElectricalSubmergedPumps,
                RigMobDemob = 2.2,
                AnnualWellInterventionCost = 3.3,
                PluggingAndAbandonment = 4.4,
                Project = project,
                ProjectId = project.Id,
            }
                .WithWellProjectCostProfile(new WellProjectCostProfile
                {
                    Currency = Currency.USD,
                    EPAVersion = "test EPA",
                    StartYear = 2030,
                    Values = new double[] { 13.4, 18.9, 34.3 }
                }
                )
                .WithDrillingSchedule(new DrillingSchedule()
                {
                    StartYear = 2030,
                    Values = new int[] { 13, 18, 34 }
                }
                );
        }

        private static WellProject CreateUpdatedWellProject(Project project)
        {
            return new WellProjectBuilder
            {
                Name = "updated name",
                ProducerCount = 4,
                GasInjectorCount = 5,
                WaterInjectorCount = 6,
                ArtificialLift = ArtificialLift.GasLift,
                RigMobDemob = 3.3,
                AnnualWellInterventionCost = 4.4,
                PluggingAndAbandonment = 5.5,
                Project = project,
                ProjectId = project.Id,
            }
                .WithWellProjectCostProfile(new WellProjectCostProfile
                {
                    Currency = Currency.NOK,
                    EPAVersion = "Updated EPA",


                    StartYear = 2030,
                    Values = new double[] { 13.4, 18.9, 34.3 }
                }
                )
                .WithDrillingSchedule(new DrillingSchedule()
                {
                    StartYear = 2030,
                    Values = new int[] { 13, 18, 34 }
                }
                );
        }
    }
}
