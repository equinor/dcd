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
    public class ExplorationAdapterShould
    {
        [Fact]
        public void ConvertExplorationDtoToDataModel()
        {
            var projectId = new Guid();
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(o => o.GetProject(projectId)).Returns(TestHelper.CreateTestProject(projectId));
            var explorationAdapter = new ExplorationAdapter(projectServiceMock.Object);
            var explorationDto = CreateTestExplorationDto(projectId);

            var exploration = explorationAdapter.Convert(explorationDto);

            Assert.Equal(explorationDto.ProjectId, exploration.Project.Id);
            Assert.Equal(explorationDto.Name, exploration.Name);
            Assert.Equal(explorationDto.WellType, exploration.WellType);
            Assert.Equal(explorationDto.RigMobDemob, exploration.RigMobDemob);

            TestHelper.CompareYearValues(explorationDto.DrillingSchedule,
                    exploration.DrillingSchedule);
            Assert.Equal(exploration.Name,
                    exploration.DrillingSchedule.Exploration.Name);

            Assert.Equal(explorationDto.CostProfile.Currency,
                    exploration.CostProfile.Currency);
            Assert.Equal(explorationDto.CostProfile.EPAVersion,
                    exploration.CostProfile.EPAVersion);
            TestHelper.CompareYearValues(explorationDto.CostProfile,
                    exploration.CostProfile);
            Assert.Equal(exploration.Name,
                    exploration.CostProfile.Exploration.Name);

            Assert.Equal(explorationDto.GAndGAdminCost.Currency,
                    exploration.GAndGAdminCost.Currency);
            Assert.Equal(explorationDto.GAndGAdminCost.EPAVersion,
                    exploration.GAndGAdminCost.EPAVersion);
            TestHelper.CompareYearValues(explorationDto.GAndGAdminCost,
                    exploration.GAndGAdminCost);
            Assert.Equal(exploration.Name,
                    exploration.GAndGAdminCost.Exploration.Name);
        }

        private ExplorationDto CreateTestExplorationDto(Guid projectId)
        {
            return new ExplorationDto
            {
                ProjectId = projectId,
                Name = "test exploration",
                WellType = WellType.Gas,
                CostProfile = new ExplorationCostProfileDto
                {
                    Currency = Currency.USD,
                    EPAVersion = "GT 1",
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2023, 1.2e6),
                        new YearValue<double> (2023, 1.3e6),
                        new YearValue<double> (2023, 1.1e6),
                    },
                },
                DrillingSchedule = new ExplorationDrillingScheduleDto
                {
                    YearValues = new List<YearValue<int>> {
                        new YearValue<int> (2023, 500),
                        new YearValue<int> (2023, 401),
                        new YearValue<int> (2023, 860),
                    },
                },
                GAndGAdminCost = new GAndGAdminCostDto
                {
                    Currency = Currency.USD,
                    EPAVersion = "GT 1",
                    YearValues = new List<YearValue<double>> {
                        new YearValue<double> (2023, 0.3e6),
                        new YearValue<double> (2023, 0.4e6),
                        new YearValue<double> (2023, 0.3e6),
                    },
                },
                RigMobDemob = 2.3e6
            };
        }
    }
}
