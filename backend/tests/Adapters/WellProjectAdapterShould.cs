using System;
using System.Collections.Generic;

using api.Adapters;
using api.Dtos;
using api.Models;

using Xunit;

namespace tests
{
    [Collection("Database collection")]
    public class WellProjectAdapterShould
    {
        [Fact]
        public void ConvertWellProjectDtoToDataModel()
        {
            // Arrange
            var projectId = new Guid();
            var wellProjectAdapter = new WellProjectAdapter();
            var wellProjectDto = CreateWellProjectDto(projectId);

            // Act
            var result = wellProjectAdapter.Convert(wellProjectDto);

            // Assert
            Assert.Equal(wellProjectDto.ProjectId, result.ProjectId);
            Assert.Equal(wellProjectDto.Name, result.Name);
            Assert.Equal(wellProjectDto.ProducerCount, result.ProducerCount);
            Assert.Equal(wellProjectDto.GasInjectorCount, result.GasInjectorCount);
            Assert.Equal(wellProjectDto.WaterInjectorCount, result.WaterInjectorCount);
            Assert.Equal(wellProjectDto.ArtificialLift, result.ArtificialLift);
            Assert.Equal(wellProjectDto.RigMobDemob, result.RigMobDemob);
            Assert.Equal(wellProjectDto.AnnualWellInterventionCost, result.AnnualWellInterventionCost);
            Assert.Equal(wellProjectDto.PluggingAndAbandonment, result.PluggingAndAbandonment);
            TestHelper.CompareCosts(wellProjectDto.CostProfile, result.CostProfile);
            TestHelper.CompareYearValues(wellProjectDto.DrillingSchedule, result.DrillingSchedule);
        }

        private WellProjectDto CreateWellProjectDto(Guid projectId)
        {
            return new WellProjectDto
            {
                ProjectId = projectId,
                Name = "Test wellProject",
                ProducerCount = 7,
                GasInjectorCount = 8,
                WaterInjectorCount = 9,
                ArtificialLift = ArtificialLift.ElectricalSubmergedPumps,
                RigMobDemob = 5.5,
                AnnualWellInterventionCost = 77.777,
                PluggingAndAbandonment = 12.34,
                CostProfile = new WellProjectCostProfileDto
                {
                    EPAVersion = "one version",
                    Currency = Currency.NOK,
                    StartYear = 2010,
                    Values = new double[] { 3.4564, 18.9, 62.3 }
                },
                DrillingSchedule = new DrillingScheduleDto
                {
                    StartYear = 2030,
                    Values = new int[] { 5, 18, 62 }
                }
            };
        }
    }
}
