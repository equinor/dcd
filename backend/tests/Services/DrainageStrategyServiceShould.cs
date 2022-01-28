using System;
using System.Linq;

using api.Models;
using api.SampleData.Builders;
using api.Services;

using Xunit;


namespace tests
{
    [Collection("Database collection")]
    public class DrainageStrategyServiceShould : IDisposable
    {
        DatabaseFixture fixture;

        public DrainageStrategyServiceShould()
        {
            fixture = new DatabaseFixture();
        }

        public void Dispose()
        {
            fixture.Dispose();
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
            drainageStrategyService.CreateDrainageStrategy(expectedStrategy);

            // Assert
            Console.WriteLine(expectedStrategy.Name);
            var actualStrategy = fixture.context.DrainageStrategies.FirstOrDefault(o => o.Name == expectedStrategy.Name);
            Assert.NotNull(actualStrategy);
            TestHelper.CompareDrainageStrategies(expectedStrategy, actualStrategy);
        }

        [Fact]
        public void ThrowArgumentExceptionIfDrainageStrategyIsNull()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);

            // Act, assert
            Assert.Throws<ArgumentException>(() => drainageStrategyService.CreateDrainageStrategy(null));
        }

        
        [Fact]
        public void ThrowArgumentExceptionIfProjectIsNull()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var expectedStrategy = CreateTestDrainageStrategy(null);

            // Act, assert
            Assert.Throws<ArgumentException>(() => drainageStrategyService.CreateDrainageStrategy(expectedStrategy));
        }

        [Fact]
        public void ThrowNotFoundInDBExceptionIfProjectNotFound()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = new ProjectBuilder {
                Id = new Guid(),
                ProjectName = "nonexistent project"
            };
            var expectedStrategy = CreateTestDrainageStrategy(project);

            // Act, assert
            Assert.Throws<NotFoundInDBException>(() => drainageStrategyService.CreateDrainageStrategy(expectedStrategy));
        }

        [Fact]
        public void AddNewDrainageStrategyToCorrectProject()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedStrategy = CreateTestDrainageStrategy(project);

            // Act
            drainageStrategyService.CreateDrainageStrategy(expectedStrategy);

            // Assert
            var drainageStrategies = fixture.context.Projects.FirstOrDefault(o => o.ProjectName == project.ProjectName).DrainageStrategies;
            var drainageStrategy = drainageStrategies.FirstOrDefault(o => o.Name == expectedStrategy.Name);
            Assert.NotNull(drainageStrategy);
        }

        [Fact]
        public void NotAddDrainageStrategyToOtherProjects()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var drainageStrategyService = new DrainageStrategyService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedStrategy = CreateTestDrainageStrategy(project);

            // Act
            drainageStrategyService.CreateDrainageStrategy(expectedStrategy);

            // Assert
            var otherProjects = fixture.context.Projects.Where(o => o.ProjectName != project.ProjectName);
            foreach(var otherProject in otherProjects)
            {
                var drainageStrategy = otherProject.DrainageStrategies.FirstOrDefault(o => o.Name == expectedStrategy.Name);
                Assert.Null(drainageStrategy);
            }
        }

        private static DrainageStrategy CreateTestDrainageStrategy(Project project)
        {
            return new DrainageStrategyBuilder()
            {
                Name = "DrainStrat Test",
                Project = project,
                NGLYield = 0.5
            }
                .WithProductionProfileGas(new ProductionProfileGasBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 2.3)
                    .WithYearValue(2031, 3.3)
                    .WithYearValue(2032, 4.4)
                )
                .WithProductionProfileOil(new ProductionProfileOilBuilder()
                {
                    Unit = VolumeUnit.BBL
                }
                    .WithYearValue(2030, 10.3)
                    .WithYearValue(2031, 13.3)
                    .WithYearValue(2032, 24.4)
                )
                .WithProductionProfileWater(new ProductionProfileWaterBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 12.34)
                    .WithYearValue(2031, 13.45)
                    .WithYearValue(2032, 14.56)
                )
                .WithProductionProfileWaterInjection(new ProductionProfileWaterInjectionBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 7.89)
                    .WithYearValue(2031, 8.91)
                    .WithYearValue(2032, 9.01)
                )
                .WithFuelFlaringAndLosses(new FuelFlaringAndLossesBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 7.89)
                    .WithYearValue(2031, 8.91)
                    .WithYearValue(2032, 9.01)
                )
                .WithNetSalesGas(new NetSalesGasBuilder()
                {
                    Unit = VolumeUnit.SM3
                }
                    .WithYearValue(2030, 11.23)
                    .WithYearValue(2031, 11.45)
                    .WithYearValue(2032, 11.56)
                    )
                .WithCo2Emissions(new Co2EmissionsBuilder()
                {
                    Unit = MassUnit.TON
                }
                    .WithYearValue(2030, 21.23)
                    .WithYearValue(2031, 22.45)
                    .WithYearValue(2032, 23.56)
                );
        }
    }
}