using System;
using System.Collections.Generic;

using api.Adapters;
using api.Dtos;
using api.Models;

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
            var drainageStrategyDto = CreateDrainageStrategyDto(projectId);

            // Act
            var result = DrainageStrategyAdapter.Convert(drainageStrategyDto);

            // Assert
            Assert.Equal(drainageStrategyDto.ProjectId, result.ProjectId);
            Assert.Equal(drainageStrategyDto.Name, result.Name);
            Assert.Equal(drainageStrategyDto.Description, result.Description);
            Assert.Equal(drainageStrategyDto.NGLYield, result.NGLYield);
            Assert.Equal(drainageStrategyDto.ArtificialLift, result.ArtificialLift);
            Assert.Equal(drainageStrategyDto.ProducerCount, result.ProducerCount);
            Assert.Equal(drainageStrategyDto.WaterInjectorCount, result.WaterInjectorCount);
            Assert.Equal(drainageStrategyDto.GasInjectorCount, result.GasInjectorCount);

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

        private DrainageStrategyDto CreateDrainageStrategyDto(Guid projectId)
        {
            return new DrainageStrategyDto
            {
                ProjectId = projectId,
                Name = "Test drainage strategy",
                Description = "Strategy test description",
                NGLYield = 0.5,
                ArtificialLift = ArtificialLift.GasLift,
                ProducerCount = 10,
                WaterInjectorCount = 12,
                GasInjectorCount = 14,

                ProductionProfileOil = new ProductionProfileOilDto
                {
                    StartYear = 2030,
                    Values = new double[] { 33.4, 18.9, 62.3 }
                },
                ProductionProfileGas = new ProductionProfileGasDto
                {
                    StartYear = 2031,
                    Values = new double[] { 33.4, 14.9, 62.3 }
                },
                ProductionProfileWater = new ProductionProfileWaterDto
                {
                    StartYear = 2040,
                    Values = new double[] { 3.4, 18.9, 62.3 }
                },
                ProductionProfileWaterInjection = new ProductionProfileWaterInjectionDto
                {
                    StartYear = 2050,
                    Values = new double[] { 3.4, 48.9, 62.3 }
                },
                FuelFlaringAndLosses = new FuelFlaringAndLossesDto
                {
                    StartYear = 2010,
                    Values = new double[] { 3.4564, 18.9, 62.3 }
                },
                NetSalesGas = new NetSalesGasDto
                {
                    StartYear = 2110,
                    Values = new double[] { 3.4564, 18.9, 67.3 }
                },
                Co2Emissions = new Co2EmissionsDto
                {
                    StartYear = 3010,
                    Values = new double[] { 34.4564, 18.9, 62.3 }
                },
            };
        }
    }
}
