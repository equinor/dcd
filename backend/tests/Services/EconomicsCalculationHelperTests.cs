using System.Linq.Expressions;

using api.Context;
using api.Dtos;
using api.Enums;
using api.Helpers;
using api.Models;
using api.Repositories;
using api.Services;
using api.Services.GenerateCostProfiles;

using NSubstitute;

using Xunit;


namespace api.Tests.Helpers
{
    public class EconomicsCalculationHelperTests
    {
        private readonly EconomicsCalculationHelper _economicsCalculationHelper;
        private readonly IExplorationRepository _explorationRepository;
        private readonly ISubstructureRepository _substructureRepository;
        private readonly ISurfRepository _surfRepository;
        private readonly ITopsideRepository _topsideRepository;
        private readonly ITransportRepository _transportRepository;
        private readonly IWellProjectRepository _wellProjectRepository;
        private readonly IDrainageStrategyService _drainageStrategyService;
        private readonly IStudyCostProfileService _studyCostProfileService;
        private readonly IDrainageStrategyRepository _drainageStrategyRepository = Substitute.For<IDrainageStrategyRepository>();
        private readonly IOpexCostProfileService _opexCostProfileService;
        private readonly ICessationCostProfileService _cessationCostProfileService;

        protected readonly DcdDbContext _context;
        private readonly ICaseService _caseService;

        public EconomicsCalculationHelperTests()
        {
            _caseService = Substitute.For<ICaseService>();
            _explorationRepository = Substitute.For<IExplorationRepository>();
            _substructureRepository = Substitute.For<ISubstructureRepository>();
            _surfRepository = Substitute.For<ISurfRepository>();
            _topsideRepository = Substitute.For<ITopsideRepository>();
            _transportRepository = Substitute.For<ITransportRepository>();
            _opexCostProfileService = Substitute.For<IOpexCostProfileService>();
            _studyCostProfileService = Substitute.For<IStudyCostProfileService>();
            _cessationCostProfileService = Substitute.For<ICessationCostProfileService>();
            _wellProjectRepository = Substitute.For<IWellProjectRepository>();
            _drainageStrategyService = Substitute.For<IDrainageStrategyService>();
            _economicsCalculationHelper = new EconomicsCalculationHelper(
            _caseService,
            _explorationRepository,
            _substructureRepository,
            _surfRepository,
            _topsideRepository,
            _transportRepository,
            _wellProjectRepository,
            _drainageStrategyService,
            _drainageStrategyRepository

            );
        }

        [Fact]
        public async Task CalculateIncome_ValidInput_ReturnsCorrectIncome()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var project = new Project
            {
                Id = Guid.NewGuid(),
                OilPriceUSD = 75,
                GasPriceNOK = 3,
                ExchangeRateUSDToNOK = 10
            };

            var caseItem = new Case
            {
                Id = caseId,
                Project = project,
                ProjectId = project.Id,
                DrainageStrategyLink = Guid.NewGuid()
            };

            var drainageStrategyId = caseItem.DrainageStrategyLink;

            var drainageStrategy = new DrainageStrategy
            {
                Id = drainageStrategyId,
                ProductionProfileOil = new ProductionProfileOil
                {
                    StartYear = 2020,
                    Values = new[] { 1000000.0, 2000000.0, 3000000.0 } // SM³
                },
                AdditionalProductionProfileOil = new AdditionalProductionProfileOil
                {
                    StartYear = 2020,
                    Values = new[] { 1000000.0, 2000000.0 } // SM³
                },
                ProductionProfileGas = new ProductionProfileGas
                {
                    StartYear = 2020,
                    Values = new[] { 1000000000.0, 2000000000.0, 3000000000.0 } // SM³
                },
                AdditionalProductionProfileGas = new AdditionalProductionProfileGas
                {
                    StartYear = 2020,
                    Values = new[] { 1000000000.0, 2000000000.0 } // SM³
                }
            };

            _caseService.GetCaseWithIncludes(caseId, Arg.Any<Expression<Func<Case, object>>>())
                .Returns(caseItem);

            _drainageStrategyService.GetDrainageStrategyWithIncludes(
                caseItem.DrainageStrategyLink,
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>()
            ).Returns(drainageStrategy);

            // Act
            await _economicsCalculationHelper.CalculateTotalIncome(caseId);

            // Assert
            var expectedFirstYearIncome = (2 * 1000000.0 * 75 * 6.29 * 10 + 2 * 1000000000.0 * 3) / 1000000;
            var expectedSecondYearIncome = (4 * 1000000.0 * 75 * 6.29 * 10 + 4 * 1000000000.0 * 3) / 1000000;
            var expectedThirdYearIncome = (3 * 1000000.0 * 75 * 6.29 * 10 + 3 * 1000000000.0 * 3) / 1000000;

            Assert.NotNull(caseItem.CalculatedTotalIncomeCostProfile);
            Assert.Equal(2020, caseItem.CalculatedTotalIncomeCostProfile.StartYear);
            Assert.Equal(3, caseItem.CalculatedTotalIncomeCostProfile.Values.Length);
            Assert.Equal(expectedFirstYearIncome, caseItem.CalculatedTotalIncomeCostProfile.Values[0], precision: 0);
            Assert.Equal(expectedSecondYearIncome, caseItem.CalculatedTotalIncomeCostProfile.Values[1], precision: 0);
            Assert.Equal(expectedThirdYearIncome, caseItem.CalculatedTotalIncomeCostProfile.Values[2], precision: 0);
        }


        [Fact]
        public async Task CalculateIncome_NullDrainageStrategy_ThrowsNullReferenceExceptionAsync()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var project = new Project { Id = Guid.NewGuid(), OilPriceUSD = 75, GasPriceNOK = 3 };

            var caseItem = new Case
            {
                Id = caseId,
                Project = project,
                ProjectId = project.Id,
                DrainageStrategyLink = Guid.NewGuid()
            };

            _caseService.GetCaseWithIncludes(caseId, Arg.Any<Expression<Func<Case, object>>>())
                .Returns(caseItem);

            _drainageStrategyService.GetDrainageStrategyWithIncludes(
                caseItem.DrainageStrategyLink,
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>()
            ).Returns((DrainageStrategy)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _economicsCalculationHelper.CalculateTotalIncome(caseId));
        }

        [Fact]
        public async Task CalculateIncome_ZeroValues_ReturnsZeroIncome()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var project = new Project
            {
                Id = Guid.NewGuid(),
                OilPriceUSD = 0,
                GasPriceNOK = 0,
                ExchangeRateUSDToNOK = 0
            };

            var caseItem = new Case
            {
                Id = caseId,
                Project = project,
                ProjectId = project.Id,
                DrainageStrategyLink = Guid.NewGuid()
            };

            var drainageStrategy = new DrainageStrategy
            {
                Id = caseItem.DrainageStrategyLink,
                ProductionProfileOil = new ProductionProfileOil
                {
                    StartYear = 2020,
                    Values = new double[] { 0.0, 0.0, 0.0 }
                },
                ProductionProfileGas = new ProductionProfileGas
                {
                    StartYear = 2020,
                    Values = new double[] { 0.0, 0.0, 0.0 }
                }
            };

            _caseService.GetCaseWithIncludes(caseId, Arg.Any<Expression<Func<Case, object>>>())
                .Returns(caseItem);

            _drainageStrategyService.GetDrainageStrategyWithIncludes(
                caseItem.DrainageStrategyLink,
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>()
            ).Returns(drainageStrategy);

            // Act
            await _economicsCalculationHelper.CalculateTotalIncome(caseId);

            // Assert
            Assert.NotNull(caseItem.CalculatedTotalIncomeCostProfile);
            Assert.Equal(2020, caseItem.CalculatedTotalIncomeCostProfile.StartYear);
            Assert.All(caseItem.CalculatedTotalIncomeCostProfile.Values, value => Assert.Equal(0.0, value));
        }

        [Fact]
        public async Task CalculateTotalCostAsync_AllProfilesProvided_ReturnsCorrectTotalCost()
        {
            var caseId = Guid.NewGuid();
            var project = new Project
            {
                Id = Guid.NewGuid(),
                OilPriceUSD = 75,
                GasPriceNOK = 3,
                ExchangeRateUSDToNOK = 10
            };

            var caseItem = new Case
            {
                Id = caseId,
                Project = project,
                ProjectId = project.Id,
                DrainageStrategyLink = Guid.NewGuid(),
                WellProjectLink = Guid.NewGuid(),
                SubstructureLink = Guid.NewGuid(),
                SurfLink = Guid.NewGuid(),
                TopsideLink = Guid.NewGuid(),
                TransportLink = Guid.NewGuid(),
                ExplorationLink = Guid.NewGuid(),
            };


            caseItem.TotalOtherStudiesCostProfile = new TotalOtherStudiesCostProfile
            {
                StartYear = 2020,
                Values = new[] { 1000.0, 1500.0, 2000.0 }
            };
            caseItem.OnshoreRelatedOPEXCostProfile = new OnshoreRelatedOPEXCostProfile
            {
                StartYear = 2020,
                Values = new[] { 500.0, 600.0, 700.0 }
            };
            caseItem.CessationWellsCost = new CessationWellsCost
            {
                StartYear = 2020,
                Values = new[] { 300.0, 400.0, 500.0 }
            };

            _studyCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(caseItem.TotalOtherStudiesCostProfile));
            _opexCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(caseItem.OnshoreRelatedOPEXCostProfile));
            _cessationCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(caseItem.CessationWellsCost));

            var substructure = new Substructure
            {
                CostProfileOverride = new SubstructureCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 70.0, 110.0, 150.0 }
                }
            };

            var surf = new Models.Surf
            {
                CostProfileOverride = new SurfCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 30.0, 60.0, 90.0 }
                }
            };

            var topside = new Topside
            {
                CostProfileOverride = new TopsideCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 50.0, 80.0, 120.0 }
                }
            };

            var transport = new Models.Transport
            {
                CostProfileOverride = new TransportCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 50.0, 70.0, 100.0 }
                }
            };

            var wellProject = new WellProject
            {
                OilProducerCostProfileOverride = new OilProducerCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 100.0, 150.0, 200.0 }
                },
                GasProducerCostProfileOverride = new GasProducerCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 50.0, 80.0, 120.0 }
                },
                WaterInjectorCostProfileOverride = new WaterInjectorCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 100.0, 100.0, 130.0 }
                },
                GasInjectorCostProfileOverride = new GasInjectorCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 50.0, 80.0, 120.0 }
                },
            };

            var exploration = new Exploration
            {
                GAndGAdminCostOverride = new GAndGAdminCostOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = new[] { 100.0, 200.0, 300.0 }
                },
                SeismicAcquisitionAndProcessing = new SeismicAcquisitionAndProcessing
                {
                    StartYear = 2020,
                    Values = new[] { 150.0, 250.0, 350.0 }
                },
                CountryOfficeCost = new CountryOfficeCost
                {
                    StartYear = 2020,
                    Values = new[] { 50.0, 100.0 }
                },
                ExplorationWellCostProfile = new ExplorationWellCostProfile
                {
                    StartYear = 2020,
                    Values = new[] { 100.0, 100.0 }
                },
                AppraisalWellCostProfile = new AppraisalWellCostProfile
                {
                    StartYear = 2020,
                    Values = new[] { 100.0, 100.0 }
                },
                SidetrackCostProfile = new SidetrackCostProfile
                {
                    StartYear = 2020,
                    Values = new[] { 100.0, 100.0 }
                }
            };

            _substructureRepository.GetSubstructure(caseItem.SubstructureLink).Returns(Task.FromResult(substructure));
            _surfRepository.GetSurf(caseItem.SurfLink).Returns(Task.FromResult(surf));
            _topsideRepository.GetTopside(caseItem.TopsideLink).Returns(Task.FromResult(topside));
            _transportRepository.GetTransport(caseItem.TransportLink).Returns(Task.FromResult(transport));
            _wellProjectRepository.GetWellProject(caseItem.WellProjectLink).Returns(Task.FromResult(wellProject));
            _explorationRepository.GetExploration(caseItem.ExplorationLink).Returns(Task.FromResult(exploration));
            _caseService.GetCaseWithIncludes(caseId, Arg.Any<Expression<Func<Case, object>>>())
                .Returns(caseItem);

            // Act
            await _economicsCalculationHelper.CalculateTotalCost(caseId);

            // Assert
            Assert.Equal(2020, caseItem.CalculatedTotalCostCostProfile.StartYear);
            Assert.Equal(3, caseItem.CalculatedTotalCostCostProfile.Values.Length);

            var expectedValues = new[] { 2900.0, 4080.0, 4880.0 };
            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], caseItem.CalculatedTotalCostCostProfile.Values[i]);
            }
        }

        [Fact]
        public async Task CalculateTotalExplorationCostAsync_ValidInput_ReturnsCorrectTotalExplorationCost()
        {
            // Arrange
            var caseItem = new Case
            {
                ExplorationLink = Guid.NewGuid()
            };

            var exploration = new Exploration
            {
                GAndGAdminCostOverride = new GAndGAdminCostOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [10.0, 20.0, 30.0]
                },
                SeismicAcquisitionAndProcessing = new SeismicAcquisitionAndProcessing
                {
                    StartYear = 2021,
                    Values = [15.0, 25.0, 35.0]
                },
                CountryOfficeCost = new CountryOfficeCost
                {
                    StartYear = 2020,
                    Values = [5.0, 10.0]
                },
                ExplorationWellCostProfile = new ExplorationWellCostProfile
                {
                    StartYear = 2021,
                    Values = [50.0, 80.0]
                },
                AppraisalWellCostProfile = new AppraisalWellCostProfile
                {
                    StartYear = 2020,
                    Values = [40.0, 60.0]
                },
                SidetrackCostProfile = new SidetrackCostProfile
                {
                    StartYear = 2021,
                    Values = [20.0, 30.0]
                }
            };

            _explorationRepository.GetExploration(caseItem.ExplorationLink)
                .Returns(Task.FromResult(exploration));

            // Act
            var result = await _economicsCalculationHelper.CalculateTotalExplorationCostAsync(caseItem);

            // Assert
            var expectedStartYear = 2020;
            var expectedValues = new double[] { 55.0, 175.0, 165.0, 35.0 };

            Assert.Equal(expectedStartYear, result.StartYear);
            Assert.Equal(expectedValues.Length, result.Values.Length);

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], result.Values[i]);
            }
        }

        [Fact]
        public void CalculateCashFlow_ValidInput_ReturnsCorrectCashFlow()
        {
            // Arrange
            var income = new TimeSeries<double>
            {
                StartYear = 2020,
                Values = [500.0, 700.0, 900.0]
            };

            var totalCost = new TimeSeries<double>
            {
                StartYear = 2020,
                Values = [200.0, 300.0, 400.0, 500.0]
            };

            var expectedStartYear = 2020;

            var expectedValues = new double[] { 300.0, 400.0, 500.0, -500.0 };

            // Act
            var result = _economicsCalculationHelper.CalculateCashFlow(income, totalCost);

            // Assert
            Assert.Equal(expectedStartYear, result.StartYear);
            Assert.Equal(expectedValues.Length, result.Values.Length);

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], result.Values[i]);
            }
        }

        [Fact]
        public void CalculateDiscountedVolume_ValidInput_ReturnsCorrectDiscountedVolume()
        {
            // Arrange
            var values = new[] { 1.0, 1.0, 1.0, 1.0, 0.5, 0.5 };
            var discountRate = 8;
            var startIndex = 0; // Assuming starting from 2030

            // Act
            var discountedVolume = _economicsCalculationHelper.CalculateDiscountedVolume(values, discountRate, startIndex);

            // Assert
            var expectedDiscountedVolume = (1.0 / Math.Pow(1 + 0.08, 1)) +
                                            (1.0 / Math.Pow(1 + 0.08, 2)) +
                                            (1.0 / Math.Pow(1 + 0.08, 3)) +
                                            (1.0 / Math.Pow(1 + 0.08, 4)) +
                                            (0.5 / Math.Pow(1 + 0.08, 5)) +
                                            (0.5 / Math.Pow(1 + 0.08, 6));
            Assert.Equal(expectedDiscountedVolume, discountedVolume, precision: 5);
        }

        [Fact]
        public async Task CalculateNPV_ValidCaseId_ReturnsCorrectNPV()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var project = new Project
            {
                DiscountRate = 8,
                OilPriceUSD = 75,
                GasPriceNOK = 3,
                ExchangeRateUSDToNOK = 10
            };

            var caseItem = new Case
            {
                Id = caseId,
                Project = project,
                DG4Date = new DateTime(2030, 1, 1),
                DrainageStrategyLink = Guid.NewGuid(),
                CalculatedTotalCostCostProfile = new CalculatedTotalCostCostProfile
                {
                    StartYear = -3,
                    Values = new[] { 2000.0, 4000.0, 1000.0, 1000.0 }
                },
                CalculatedTotalIncomeCostProfile = new CalculatedTotalIncomeCostProfile
                {
                    StartYear = 0,
                    Values = new[] { 6217.5, 6217.5, 6217.5, 6217.5, 2958.75, 2958.75 }
                },
            };

            _caseService.GetCaseWithIncludes(caseId, Arg.Any<Expression<Func<Case, object>>>())
                .Returns(caseItem);

            await _economicsCalculationHelper.CalculateNPV(caseId);

            var actualNpvValue = 10816.2;
            Assert.Equal(actualNpvValue, caseItem.NPV, precision: 1);
        }


        [Fact]
        public async Task CalculateBreakEvenOilPrice_ValidCaseId_ReturnsCorrectBreakEvenPrice()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var project = new Project
            {
                DiscountRate = 8,
                OilPriceUSD = 75,
                GasPriceNOK = 3,
                ExchangeRateUSDToNOK = 10
            };

            var caseItem = new Case
            {
                Id = caseId,
                Project = project,
                DG4Date = new DateTime(2030, 1, 1),
                DrainageStrategyLink = Guid.NewGuid(),
                CalculatedTotalCostCostProfile = new CalculatedTotalCostCostProfile
                {
                    StartYear = 2027,
                    Values = new[] { 2000.0, 4000.0, 1000.0, 1000.0 }
                },
            };

            var drainageStrategy = new DrainageStrategy
            {
                Id = caseItem.DrainageStrategyLink,

                ProductionProfileOil = new ProductionProfileOil
                {
                    StartYear = 2030,
                    Values = new[] { 1000000.0, 1000000.0, 1000000.0, 1000000.0, 500000.0, 500000.0 }
                },
                ProductionProfileGas = new ProductionProfileGas
                {
                    StartYear = 2030,
                    Values = new[] { 500000000.0, 500000000.0, 500000000.0, 500000000.0, 200000000.0, 200000000.0 }
                },
            };

            _caseService.GetCaseWithIncludes(caseId, Arg.Any<Expression<Func<Case, object>>>())
                .Returns(caseItem);

            _drainageStrategyService.GetDrainageStrategyWithIncludes(
                caseItem.DrainageStrategyLink,
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>(),
                Arg.Any<Expression<Func<DrainageStrategy, object>>>()
            ).Returns(drainageStrategy);

            // Act
            await _economicsCalculationHelper.CalculateBreakEvenOilPrice(caseId);

            // Assert
            var expectedBreakEvenPrice = 262.9;
            Assert.Equal(expectedBreakEvenPrice, caseItem.BreakEven, precision: 1);
        }

    }
}
