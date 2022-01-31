using System;
using System.Collections.Generic;

using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Moq;

using Xunit;

namespace tests
{
    [Collection("Database collection")]
    public class DrainageStrategyAdapterShould
    {
        [Fact]
        public void ConvertDrainageStrategyDtoToDataModel()
        {
            // Arrange
            var projectId = new Guid();
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(o => o.GetProject(projectId)).Returns(CreateTestProject(projectId));
            var drainageStrategyAdapter = new DrainageStrategyAdapter(projectServiceMock.Object);
            var drainageStrategyDto = CreateDrainageStrategyDto(projectId);

            // Act
            var result = drainageStrategyAdapter.Convert(drainageStrategyDto);

            // Assert
            Assert.Equal(drainageStrategyDto.ProjectId, result.Project.Id);
            Assert.Equal(drainageStrategyDto.Name, result.Name);
            Assert.Equal(drainageStrategyDto.Description, result.Description);
            Assert.Equal(drainageStrategyDto.NGLYield, result.NGLYield);
            Assert.Equal(drainageStrategyDto.ArtificialLift, result.ArtificialLift);

            Assert.Equal(drainageStrategyDto.Name, result.ProductionProfileOil.DrainageStrategy.Name);
            TestHelper.CompareYearValues<double>(drainageStrategyDto.ProductionProfileOil, result.ProductionProfileOil);

            Assert.Equal(drainageStrategyDto.Name, result.ProductionProfileGas.DrainageStrategy.Name);
            TestHelper.CompareYearValues<double>(drainageStrategyDto.ProductionProfileGas, result.ProductionProfileGas);

            Assert.Equal(drainageStrategyDto.Name, result.ProductionProfileWater.DrainageStrategy.Name);
            TestHelper.CompareYearValues<double>(drainageStrategyDto.ProductionProfileWater, result.ProductionProfileWater);

            Assert.Equal(drainageStrategyDto.Name, result.ProductionProfileWaterInjection.DrainageStrategy.Name);
            TestHelper.CompareYearValues<double>(drainageStrategyDto.ProductionProfileWaterInjection, result.ProductionProfileWaterInjection);

            Assert.Equal(drainageStrategyDto.Name, result.FuelFlaringAndLosses.DrainageStrategy.Name);
            TestHelper.CompareYearValues<double>(drainageStrategyDto.FuelFlaringAndLosses, result.FuelFlaringAndLosses);

            Assert.Equal(drainageStrategyDto.Name, result.NetSalesGas.DrainageStrategy.Name);
            TestHelper.CompareYearValues<double>(drainageStrategyDto.NetSalesGas, result.NetSalesGas);

            Assert.Equal(drainageStrategyDto.Name, result.Co2Emissions.DrainageStrategy.Name);
            TestHelper.CompareYearValues<double>(drainageStrategyDto.Co2Emissions, result.Co2Emissions);
        }

        private Project CreateTestProject(Guid projectId)
        {
            return new Project
            {
                Id = projectId,
                ProjectName = "Test project"
            };
        }

        private DrainageStrategyDto CreateDrainageStrategyDto(Guid projectId)
        {
            return new DrainageStrategyDto
            {
                ProjectId = projectId,
                Name = "Test drainage strategy",
                Description = "Strategy test description",
                NGLYield = 0.5,
                ArtificialLift = ArtificialLift.GasLift,
                ProductionProfileOil = new ProductionProfileOilDto
                {
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2050, 5.5),
                        new YearValue<double> (2051, 5.3),
                        new YearValue<double> (2052, 4.5),
                    }
                },
                ProductionProfileGas = new ProductionProfileGasDto
                {
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2050, 5.5),
                        new YearValue<double> (2051, 5.3),
                        new YearValue<double> (2052, 4.5),
                    }
                },
                ProductionProfileWater = new ProductionProfileWaterDto
                {
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2050, 1.5),
                        new YearValue<double> (2051, 1.3),
                        new YearValue<double> (2052, 1.5),
                    }
                },
                ProductionProfileWaterInjection = new ProductionProfileWaterInjectionDto
                {
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2050, 2.5),
                        new YearValue<double> (2051, 2.3),
                        new YearValue<double> (2052, 2.5),
                    }
                },
                FuelFlaringAndLosses = new FuelFlaringAndLossesDto
                {
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2050, 3.5),
                        new YearValue<double> (2051, 3.3),
                        new YearValue<double> (2052, 3.5),
                    }
                },
                NetSalesGas = new NetSalesGasDto
                {
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2050, 9.5),
                        new YearValue<double> (2051, 9.3),
                        new YearValue<double> (2052, 9.5),
                    }
                },
                Co2Emissions = new Co2EmissionsDto
                {
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2050, 8.5),
                        new YearValue<double> (2051, 8.3),
                        new YearValue<double> (2052, 8.5),
                    }
                },
            };
        }
    }
}
