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
    public class DrainageStrategyServiceShould : IDisposable
    {
        private readonly DatabaseFixture fixture;

        public DrainageStrategyServiceShould()
        {
            fixture = new DatabaseFixture();
        }

        public void Dispose()
        {
            fixture.Dispose();
        }

        [Fact]
        public void GetDrainageStrategies()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedStrategies = fixture.context.DrainageStrategies.ToList().Where(o => o.Project.Id == project.Id);

            // Act
            var actualStrategies = drainageStrategyService.GetDrainageStrategies(project.Id);

            // Assert
            Assert.Equal(expectedStrategies.Count(), actualStrategies.Count());
            var drainageStrategiesExpectedAndActual = expectedStrategies.OrderBy(d => d.Name)
                .Zip(actualStrategies.OrderBy(d => d.Name));
            foreach (var drainageStrategyPair in drainageStrategiesExpectedAndActual)
            {
                TestHelper.CompareDrainageStrategies(drainageStrategyPair.First, drainageStrategyPair.Second);
            }
        }

        [Fact]
        public void CreateNewDrainageStrategy()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
            var caseId = project.Cases.FirstOrDefault().Id;
            var expectedStrategy = CreateTestDrainageStrategy(project);

            // Act
            var projectResult = drainageStrategyService.CreateDrainageStrategy(DrainageStrategyDtoAdapter.Convert(expectedStrategy), caseId);

            // Assert
            var actualStrategy = projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == expectedStrategy.Name);
            Assert.NotNull(actualStrategy);
            TestHelper.CompareDrainageStrategies(expectedStrategy, actualStrategy);
            var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
            Assert.Equal(actualStrategy.Id, case_.DrainageStrategyLink);
        }

        [Fact]
        public void ThrowNotInDatabaseExceptionWhenCreatingDrainageStrategyWithBadProjectId()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
            var caseId = project.Cases.FirstOrDefault().Id;
            var expectedStrategy = CreateTestDrainageStrategy(new Project { Id = new Guid() });

            // Act, assert
            Assert.Throws<NotFoundInDBException>(() =>
            drainageStrategyService.CreateDrainageStrategy(DrainageStrategyDtoAdapter.Convert(expectedStrategy), caseId));
        }

        [Fact]
        public void ThrowNotFoundInDatabaseExceptionWhenCreatingDrainageStrategyWithBadCaseId()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
            var expectedStrategy = CreateTestDrainageStrategy(project);

            // Act, assert
            Assert.Throws<NotFoundInDBException>(() =>
            drainageStrategyService.CreateDrainageStrategy(DrainageStrategyDtoAdapter.Convert(expectedStrategy), new Guid()));
        }

        [Fact]
        public void DeleteDrainageStrategy()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var drainageStrategyToDelete = CreateTestDrainageStrategy(project);
            fixture.context.DrainageStrategies.Add(drainageStrategyToDelete);
            fixture.context.Cases.Add(new Case
            {
                Project = project,
                DrainageStrategyLink = drainageStrategyToDelete.Id
            });
            fixture.context.SaveChanges();

            // Act
            var projectResult = drainageStrategyService.DeleteDrainageStrategy(drainageStrategyToDelete.Id);

            // Assert
            var actualDrainageStrategy = projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == drainageStrategyToDelete.Name);
            Assert.Null(actualDrainageStrategy);
            var casesWithDrainageStrategyLink = projectResult.Cases.Where(o => o.DrainageStrategyLink == drainageStrategyToDelete.Id);
            Assert.Empty(casesWithDrainageStrategyLink);
        }

        [Fact]
        public void ThrowArgumentExceptionIfTryingToDeleteNonExistentDrainageStrategy()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var drainageStrategyToDelete = CreateTestDrainageStrategy(project);
            fixture.context.DrainageStrategies.Add(drainageStrategyToDelete);
            fixture.context.SaveChanges();

            // Act, assert
            Assert.Throws<ArgumentException>(() => drainageStrategyService.DeleteDrainageStrategy(new Guid()));
        }

        [Fact]
        public void UpdateDrainageStrategy()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var oldStrategy = CreateTestDrainageStrategy(project);
            fixture.context.DrainageStrategies.Add(oldStrategy);
            fixture.context.SaveChanges();
            var updatedStrategy = CreateUpdatedDrainageStrategy(project, oldStrategy);

            // Act
            var projectResult = drainageStrategyService.UpdateDrainageStrategy(DrainageStrategyDtoAdapter.Convert(updatedStrategy));

            // Assert
            var actualStrategy = projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == updatedStrategy.Name);
            Assert.NotNull(actualStrategy);
            TestHelper.CompareDrainageStrategies(updatedStrategy, actualStrategy);
        }

        [Fact]
        public void ThrowArgumentExceptionIfTryingToUpdateNonExistentDrainageStrategy()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var oldStrategy = CreateTestDrainageStrategy(project);
            fixture.context.DrainageStrategies.Add(oldStrategy);
            fixture.context.SaveChanges();
            var updatedStrategy = CreateUpdatedDrainageStrategy(project, oldStrategy);

            //     // Act, assert
            //     Assert.Throws<ArgumentException>(() => drainageStrategyService.UpdateDrainageStrategy(updatedStrategy));
        }

        private static DrainageStrategy CreateTestDrainageStrategy(Project project)
        {
            return new DrainageStrategyBuilder
            {
                Name = "DrainStrat Test",
                Description = "Some description of the strategy",
                Project = project,
                ProjectId = project.Id,
                NGLYield = 0.5,
                WaterInjectorCount = 20,
                GasInjectorCount = 22,
                ProducerCount = 24,
                ArtificialLift = ArtificialLift.ElectricalSubmergedPumps,
            }
               .WithProductionProfileGas(new ProductionProfileGas()
               {
                   StartYear = 2030,
                   Values = new double[] { 2.3, 3.3, 4.4 }
               }
                )
                .WithProductionProfileOil(new ProductionProfileOil()
                {
                    StartYear = 2030,
                    Values = new double[] { 10.3, 13.3, 24.4 }
                }
                )
                .WithProductionProfileWater(new ProductionProfileWater()
                {
                    StartYear = 2030,
                    Values = new double[] { 12.34, 13.45, 14.56 }
                }
                )
                .WithProductionProfileWaterInjection(new ProductionProfileWaterInjection()
                {
                    StartYear = 2030,
                    Values = new double[] { 7.89, 8.91, 9.01 }
                }
                )
                .WithFuelFlaringAndLosses(new FuelFlaringAndLosses()
                {
                    StartYear = 2030,
                    Values = new double[] { 8.45, 4.78, 8, 74 }
                }
                )
                .WithNetSalesGas(new NetSalesGas()
                {
                    StartYear = 2030,
                    Values = new double[] { 3.4, 8.9, 2.3 }
                }
                )
                .WithCo2Emissions(new Co2Emissions()
                {
                    StartYear = 2030,
                    Values = new double[] { 33.4, 18.9, 62.3 }
                }
                );
        }

        private static DrainageStrategy CreateUpdatedDrainageStrategy(Project project, DrainageStrategy oldDrainage)
        {
            return new DrainageStrategyBuilder
            {
                Id = oldDrainage.Id,
                Name = "Updated strategy",
                Description = "Updated description",
                Project = project,
                ProjectId = project.Id,
                NGLYield = 1.5,
                WaterInjectorCount = 23,
                GasInjectorCount = 23,
                ProducerCount = 21,
                ArtificialLift = ArtificialLift.GasLift,
            }
                .WithProductionProfileGas(new ProductionProfileGas()
                {
                    StartYear = 2130,
                    Values = new double[] { 2.3, 23.3, 4.4 }
                }
                )
                .WithProductionProfileOil(new ProductionProfileOil()
                {
                    StartYear = 2230,
                    Values = new double[] { 10.23, 13.3, 24.4 }
                }
                )
                .WithProductionProfileWater(new ProductionProfileWater()
                {
                    StartYear = 2230,
                    Values = new double[] { 12.34, 13.425, 14.56 }
                }
                )
                .WithProductionProfileWaterInjection(new ProductionProfileWaterInjection()
                {
                    StartYear = 20230,
                    Values = new double[] { 7.89, 28.91, 9.01 }
                }
                )
                .WithFuelFlaringAndLosses(new FuelFlaringAndLosses()
                {
                    StartYear = 20230,
                    Values = new double[] { 8.425, 4.78, 8, 74 }
                }
                )
                .WithNetSalesGas(new NetSalesGas()
                {
                    StartYear = 1030,
                    Values = new double[] { 3.4, 8.9, 2.3 }
                }
                )
                .WithCo2Emissions(new Co2Emissions()
                {
                    StartYear = 2034,
                    Values = new double[] { 33.4, 181.9, 62.3 }
                }
                );
        }
    }
}
