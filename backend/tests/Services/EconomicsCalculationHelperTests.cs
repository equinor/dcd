using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using api.Dtos;
using api.Helpers;
using api.Models;
using api.Repositories;
using api.Services;
using api.Services.GenerateCostProfiles;
using NSubstitute;

namespace api.Tests.Helpers
{
    public class EconomicsCalculationHelperTests
    {
        private readonly EconomicsCalculationHelper _economicsCalculationHelper;

        private readonly IStudyCostProfileService _studyCostProfileService = Substitute.For<IStudyCostProfileService>();
        private readonly IOpexCostProfileService _opexCostProfileService = Substitute.For<IOpexCostProfileService>();
        private readonly ICessationCostProfileService _cessationCostProfileService = Substitute.For<ICessationCostProfileService>();
        private readonly IExplorationRepository _explorationRepository = Substitute.For<IExplorationRepository>();
        private readonly ISubstructureRepository _substructureRepository = Substitute.For<ISubstructureRepository>();
        private readonly ISurfRepository _surfRepository = Substitute.For<ISurfRepository>();
        private readonly ITopsideRepository _topsideRepository = Substitute.For<ITopsideRepository>();
        private readonly ITransportRepository _transportRepository = Substitute.For<ITransportRepository>();
        private readonly IWellProjectRepository _wellProjectRepository = Substitute.For<IWellProjectRepository>();
        private readonly ICo2IntensityTotalService _co2IntensityTotalService = Substitute.For<ICo2IntensityTotalService>();
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();

        public EconomicsCalculationHelperTests()
        {
            _economicsCalculationHelper = new EconomicsCalculationHelper(
                _studyCostProfileService,
                _opexCostProfileService,
                _cessationCostProfileService,
                _explorationRepository,
                _substructureRepository,
                _surfRepository,
                _topsideRepository,
                _transportRepository,
                _wellProjectRepository,
                _co2IntensityTotalService,
                _projectService
            );
        }

        [Fact]
        public void CalculateIncome_ValidInput_ReturnsCorrectIncome()
        {
            // Arrange
            var drainageStrategy = new DrainageStrategy
            {
                ProductionProfileOil = new ProductionProfileOil
                {
                    StartYear = 2020,
                    Values = [1000000.0, 2000000.0, 3000000.0]
                },
                AdditionalProductionProfileOil = new AdditionalProductionProfileOil
                {
                    StartYear = 2020,
                    Values = [1000000.0, 2000000.0]
                },
                ProductionProfileGas = new ProductionProfileGas
                {
                    StartYear = 2020,
                    Values = [1000000000.0, 2000000000.0, 3000000000.0]
                },
                AdditionalProductionProfileGas = new AdditionalProductionProfileGas
                {
                    StartYear = 2020,
                    Values = [1000000000.0, 2000000000.0]
                }
            };

            var project = new Project
            {
                OilPrice = 75,
                GasPrice = 0.3531,
            };

            // Act
            var income = EconomicsCalculationHelper.CalculateIncome(drainageStrategy, project);

            // Assert
            Assert.Equal(2020, income.StartYear);
            Assert.Equal(3, income.Values.Length);

            // Formulas
            //income = income oil + income gas
            //income oil (dollar) = production profile oil (in bbls)* oil price (dollar/bbl)
            // production profile oil (in bbls) = production profile oil (in SM3) * cubicMetersToBarrelsFactor (6.29)
            //income gas (dollar) = net sales gas (in SM3) * gas price

            // first year oil income = 2*6.29*75 = 943.5 (MNOK)
            // first year gas income = 2*0.3531 = 706.2 (MNOK)
            // first year total income = 1649.7 (MNOK)

            // Assert values for each year (Divides by 1 mill because income is later converted to show income in millions)
            // oil cubics in million * oil price + gas cubics in billions * gas price
            double expectedFirstYearIncome = (2 * 1000000.0 * 75 + 2 * 1000000000.0 * 0.3531) / 1000000;
            double expectedSecondYearIncome = (2 * 2000000.0 * 75 + 2 * 2000000000.0 * 0.3531) / 1000000;
            double expectedThirdYearIncome = (2 * 3000000.0 * 75 + 2 * 3000000000.0 * 0.3531) / 1000000;

            Assert.Equal(expectedFirstYearIncome, income.Values[0], precision: 0);
            Assert.Equal(expectedSecondYearIncome, income.Values[1], precision: 0);
            Assert.Equal(expectedThirdYearIncome, income.Values[2], precision: 0);
        }


        //         [Fact]
        //         public async Task CalculateTotalCostAsync_ValidInput_ReturnsCorrectTotalCost()
        //         {
        //             // Arrange
        //             var caseItem = new Case { Id = 1 };

        //             _studyCostProfileService.Generate(caseItem.Id).Returns(new StudyCostProfileDto
        //             {
        //                 StudyCostProfileDto = new TimeSeries<double>
        //                 {
        //                     StartYear = 2020,
        //                     Values = new double[] { 100, 200, 300 }
        //                 }
        //             });

        //             _opexCostProfileService.Generate(caseItem.Id).Returns(new OpexCostProfileDto
        //             {
        //                 OpexCostProfileDto = new TimeSeries<double>
        //                 {
        //                     StartYear = 2020,
        //                     Values = new double[] { 50, 60, 70 }
        //                 }
        //             });

        //             _cessationCostProfileService.Generate(caseItem.Id).Returns(new CessationCostDto
        //             {
        //                 CessationCostDto = new TimeSeries<double>
        //                 {
        //                     StartYear = 2020,
        //                     Values = new double[] { 30, 40, 50 }
        //                 }
        //             });

        //             // Act
        //             var totalCost = await _helper.CalculateTotalCostAsync(caseItem);

        //             // Assert
        //             Assert.Equal(2020, totalCost.StartYear);
        //             Assert.Equal(3, totalCost.Values.Length);
        //             // Add more assertions to verify the correctness of total cost
        //         }
    }
}