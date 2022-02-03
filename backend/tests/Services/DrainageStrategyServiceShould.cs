using System;
using System.Linq;

using api.Models;
using api.SampleData.Builders;
using api.SampleData.Generators;
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
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedStrategy = CreateTestDrainageStrategy(project);

            // Act
            var projectResult = drainageStrategyService.CreateDrainageStrategy(expectedStrategy);

            // Assert
            var actualStrategy = projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == expectedStrategy.Name);
            Assert.NotNull(actualStrategy);
            TestHelper.CompareDrainageStrategies(expectedStrategy, actualStrategy);
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
            fixture.context.SaveChanges();

            // Act
            var projectResult = drainageStrategyService.DeleteDrainageStrategy(drainageStrategyToDelete.Id);

            // Assert
            var actualDrainageStrategy = projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == drainageStrategyToDelete.Name);
            Assert.Null(actualDrainageStrategy);
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
            var updatedStrategy = CreateUpdatedDrainageStrategy(project);

            // Act
            var projectResult = drainageStrategyService.UpdateDrainageStrategy(oldStrategy.Id, updatedStrategy);

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
            var updatedStrategy = CreateUpdatedDrainageStrategy(project);

            // Act, assert
            Assert.Throws<ArgumentException>(() => drainageStrategyService.UpdateDrainageStrategy(new Guid(), updatedStrategy));
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
                .WithProductionProfileGas(new ProductionProfileGasBuilder()
                    .WithYearValue(2030, 2.3)
                    .WithYearValue(2031, 3.3)
                    .WithYearValue(2032, 4.4)
                )
                .WithProductionProfileOil(new ProductionProfileOilBuilder()
                    .WithYearValue(2030, 10.3)
                    .WithYearValue(2031, 13.3)
                    .WithYearValue(2032, 24.4)
                )
                .WithProductionProfileWater(new ProductionProfileWaterBuilder()
                    .WithYearValue(2030, 12.34)
                    .WithYearValue(2031, 13.45)
                    .WithYearValue(2032, 14.56)
                )
                .WithProductionProfileWaterInjection(new ProductionProfileWaterInjectionBuilder()
                    .WithYearValue(2030, 7.89)
                    .WithYearValue(2031, 8.91)
                    .WithYearValue(2032, 9.01)
                )
                .WithFuelFlaringAndLosses(new FuelFlaringAndLossesBuilder()
                    .WithYearValue(2030, 7.89)
                    .WithYearValue(2031, 8.91)
                    .WithYearValue(2032, 9.01)
                )
                .WithNetSalesGas(new NetSalesGasBuilder()
                    .WithYearValue(2030, 11.23)
                    .WithYearValue(2031, 11.45)
                    .WithYearValue(2032, 11.56)
                    )
                .WithCo2Emissions(new Co2EmissionsBuilder()
                    .WithYearValue(2030, 21.23)
                    .WithYearValue(2031, 22.45)
                    .WithYearValue(2032, 23.56)
                );
        }

        private static DrainageStrategy CreateUpdatedDrainageStrategy(Project project)
        {
            return new DrainageStrategyBuilder
            {
                Name = "Updated strategy",
                Description = "Updated description",
                Project = project,
                ProjectId = project.Id,
                NGLYield = 1.5,
                WaterInjectorCount = 21,
                GasInjectorCount = 23,
                ProducerCount = 25,
                ArtificialLift = ArtificialLift.GasLift,
            }
                .WithProductionProfileGas(new ProductionProfileGasBuilder()
                    .WithYearValue(2030, 12.3)
                    .WithYearValue(2031, 13.3)
                    .WithYearValue(2032, 14.4)
                )
                .WithProductionProfileOil(new ProductionProfileOilBuilder()
                    .WithYearValue(2030, 20.3)
                    .WithYearValue(2031, 23.3)
                    .WithYearValue(2032, 34.4)
                )
                .WithProductionProfileWater(new ProductionProfileWaterBuilder()
                    .WithYearValue(2030, 22.34)
                    .WithYearValue(2031, 23.45)
                    .WithYearValue(2032, 24.56)
                )
                .WithProductionProfileWaterInjection(new ProductionProfileWaterInjectionBuilder()
                    .WithYearValue(2030, 17.89)
                    .WithYearValue(2031, 18.91)
                    .WithYearValue(2032, 19.01)
                )
                .WithFuelFlaringAndLosses(new FuelFlaringAndLossesBuilder()
                    .WithYearValue(2030, 17.89)
                    .WithYearValue(2031, 18.91)
                    .WithYearValue(2032, 19.01)
                )
                .WithNetSalesGas(new NetSalesGasBuilder()
                    .WithYearValue(2030, 21.23)
                    .WithYearValue(2031, 21.45)
                    .WithYearValue(2032, 21.56)
                    )
                .WithCo2Emissions(new Co2EmissionsBuilder()
                    .WithYearValue(2030, 31.23)
                    .WithYearValue(2031, 32.45)
                    .WithYearValue(2032, 33.56)
                );
        }
    }
}
