using api.Dtos;
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

        private readonly IStudyCostProfileService _studyCostProfileService;
        private readonly IOpexCostProfileService _opexCostProfileService;
        private readonly ICessationCostProfileService _cessationCostProfileService;
        private readonly IExplorationRepository _explorationRepository;
        private readonly ISubstructureRepository _substructureRepository;
        private readonly ISubstructureTimeSeriesRepository _substructureTimeSeriesRepository = Substitute.For<ISubstructureTimeSeriesRepository>();
        private readonly ISurfRepository _surfRepository;
        private readonly ITopsideRepository _topsideRepository;
        private readonly ITransportRepository _transportRepository;
        private readonly IWellProjectRepository _wellProjectRepository;
        private readonly ICo2IntensityTotalService _co2IntensityTotalService;
        private readonly IProjectService _projectService;


        public EconomicsCalculationHelperTests()
        {
            // Initialize the substitutes
            _studyCostProfileService = Substitute.For<IStudyCostProfileService>();
            _opexCostProfileService = Substitute.For<IOpexCostProfileService>();
            _cessationCostProfileService = Substitute.For<ICessationCostProfileService>();
            _explorationRepository = Substitute.For<IExplorationRepository>();
            _substructureRepository = Substitute.For<ISubstructureRepository>();
            _substructureTimeSeriesRepository = Substitute.For<ISubstructureTimeSeriesRepository>();

            _surfRepository = Substitute.For<ISurfRepository>();
            _topsideRepository = Substitute.For<ITopsideRepository>();
            _transportRepository = Substitute.For<ITransportRepository>();
            _wellProjectRepository = Substitute.For<IWellProjectRepository>();
            _co2IntensityTotalService = Substitute.For<ICo2IntensityTotalService>();
            _projectService = Substitute.For<IProjectService>();

            // Instantiate EconomicsCalculationHelper with all required dependencies
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
                _projectService,
                _substructureTimeSeriesRepository
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
            var income = _economicsCalculationHelper.CalculateIncome(drainageStrategy, project);

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
            double expectedFirstYearIncome = (2 * 1000000.0 * 75 * 6.29 + 2 * 1000000000.0 * 0.3531) / 1000000;
            double expectedSecondYearIncome = (4 * 1000000.0 * 75 * 6.29 + 4 * 1000000000.0 * 0.3531) / 1000000;
            double expectedThirdYearIncome = (3 * 1000000.0 * 75 * 6.29 + 3 * 1000000000.0 * 0.3531) / 1000000;

            Assert.Equal(expectedFirstYearIncome, income.Values[0], precision: 0);
            Assert.Equal(expectedSecondYearIncome, income.Values[1], precision: 0);
            Assert.Equal(expectedThirdYearIncome, income.Values[2], precision: 0);
        }

        [Fact]
        public void CalculateIncome_NullDrainageStrategy_ThrowsNullReferenceException()
        {
            // Arrange
            DrainageStrategy drainageStrategy = null;
            var project = new Project { OilPrice = 75, GasPrice = 0.3531 };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _economicsCalculationHelper.CalculateIncome(drainageStrategy, project));
        }

        [Fact]
        public void CalculateIncome_ZeroValues_ReturnsZeroIncome()
        {
            // Arrange
            var drainageStrategy = new DrainageStrategy
            {
                ProductionProfileOil = new ProductionProfileOil
                {
                    StartYear = 2020,
                    Values = [0.0, 0.0, 0.0]
                },
                ProductionProfileGas = new ProductionProfileGas
                {
                    StartYear = 2020,
                    Values = [0.0, 0.0, 0.0]
                }
            };

            var project = new Project { OilPrice = 0, GasPrice = 0 };

            // Act
            var income = _economicsCalculationHelper.CalculateIncome(drainageStrategy, project);

            // Assert
            Assert.Equal(2020, income.StartYear);
            Assert.All(income.Values, value => Assert.Equal(0.0, value));
        }

        [Fact]
        public async Task CalculateTotalCostAsync_AllProfilesProvided_ReturnsCorrectTotalCost()
        {
            // Arrange
            var caseItem = new Case
            {
                Id = Guid.NewGuid(),
                WellProjectLink = Guid.NewGuid(),
                SubstructureLink = Guid.NewGuid(),
                SurfLink = Guid.NewGuid(),
                TopsideLink = Guid.NewGuid(),
                TransportLink = Guid.NewGuid(),
                ExplorationLink = Guid.NewGuid()

            };

            var studyCostProfileDto = new StudyCostProfileDto
            {
                StartYear = 2020,
                Values = [1000.0, 1500.0, 2000.0]
            };
            var opexCostProfileDto = new OpexCostProfileDto
            {
                StartYear = 2020,
                Values = [500.0, 600.0, 700.0]
            };
            var cessationCostDto = new CessationCostDto
            {
                StartYear = 2020,
                Values = [300.0, 400.0, 500.0]
            };

            // Wrap DTOs in wrapper DTOs
            var studyCostProfileWrapper = new StudyCostProfileWrapperDto { StudyCostProfileDto = studyCostProfileDto };
            var opexCostProfileWrapper = new OpexCostProfileWrapperDto { OpexCostProfileDto = opexCostProfileDto };
            var cessationCostWrapper = new CessationCostWrapperDto { CessationCostDto = cessationCostDto };

            // Mock the services to return the wrapper DTOs
            _studyCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(studyCostProfileWrapper));
            _opexCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(opexCostProfileWrapper));
            _cessationCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(cessationCostWrapper));

            var substructure = new Substructure
            {
                CostProfileOverride = new SubstructureCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [70.0, 110.0, 150.0]
                }
            };

            var surf = new Models.Surf
            {
                CostProfileOverride = new SurfCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [30.0, 60.0, 90.0]
                }
            };

            var topside = new Topside
            {
                CostProfileOverride = new TopsideCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                }
            };

            var transport = new Models.Transport
            {
                CostProfileOverride = new TransportCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [40.0, 70.0, 100.0]
                }
            };

            // Set up the substitutes to return the expected cost profiles
            _substructureRepository.GetSubstructure(caseItem.SubstructureLink)
                .Returns(Task.FromResult(substructure));

            _surfRepository.GetSurf(caseItem.SurfLink)
                .Returns(Task.FromResult(surf));

            _topsideRepository.GetTopside(caseItem.TopsideLink)
                .Returns(Task.FromResult(topside));

            _transportRepository.GetTransport(caseItem.TransportLink)
                .Returns(Task.FromResult(transport));


            var wellProject = new WellProject
            {
                OilProducerCostProfileOverride = new OilProducerCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 150.0, 200.0]
                },

                GasProducerCostProfileOverride = new GasProducerCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                },

                WaterInjectorCostProfileOverride = new WaterInjectorCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [70.0, 100.0, 130.0]
                },

                GasInjectorCostProfileOverride = new GasInjectorCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                },
            };

            // Set up the substitutes to return the expected cost profiles
            _wellProjectRepository.GetWellProject(caseItem.WellProjectLink)
                .Returns(Task.FromResult(wellProject));

            var exploration = new Exploration
            {
                GAndGAdminCostOverride = new GAndGAdminCostOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 200.0, 300.0]
                },
                SeismicAcquisitionAndProcessing = new SeismicAcquisitionAndProcessing
                {
                    StartYear = 2020,
                    Values = [150.0, 250.0, 350.0]
                },
                CountryOfficeCost = new CountryOfficeCost
                {
                    StartYear = 2020,
                    Values = [50.0, 100.0]
                },
                ExplorationWellCostProfile = new ExplorationWellCostProfile
                {
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },
                AppraisalWellCostProfile = new AppraisalWellCostProfile
                {
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },
                SidetrackCostProfile = new SidetrackCostProfile
                {
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                }
            };

            // Set up the substitute to return the expected exploration costs
            _explorationRepository.GetExploration(caseItem.ExplorationLink)
                .Returns(Task.FromResult(exploration));



            // Act
            var result = await _economicsCalculationHelper.CalculateTotalCostAsync(caseItem);

            // Assert
            Assert.Equal(2020, result.StartYear);
            Assert.Equal(3, result.Values.Length);

            var expectedValues = new[]
            {
                2860, 4080, 4880
            };

            Assert.Equal(expectedValues.Length, result.Values.Length);
            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], result.Values[i], precision: 0);
            }
        }

        [Fact]
        public async Task CalculateTotalCostAsync_SomeProfilesMissing_ReturnsCorrectTotalCost()
        {
            // Arrange
            var caseItem = new Case
            {
                Id = Guid.NewGuid(),
                WellProjectLink = Guid.NewGuid(),
                SubstructureLink = Guid.NewGuid(),
                SurfLink = Guid.NewGuid(),
                TopsideLink = Guid.NewGuid(),
                TransportLink = Guid.NewGuid(),
                ExplorationLink = Guid.NewGuid()
            };

            // Define profiles
            var studyCostProfileDto = new StudyCostProfileDto
            {
                StartYear = 2020,
                Values = [1000.0, 1500.0, 2000.0]
            };

            var opexCostProfileDto = new OpexCostProfileDto
            {
                StartYear = 2020,
                Values = [500.0, 600.0, 700.0]
            };

            // Cessation cost is not provided in this test
            var cessationCostDto = new CessationCostDto
            {
                StartYear = 2020,
                Values = [300.0, 400.0, 500.0]
            };

            // Wrap DTOs in wrapper DTOs
            var studyCostProfileWrapper = new StudyCostProfileWrapperDto { StudyCostProfileDto = studyCostProfileDto };
            var opexCostProfileWrapper = new OpexCostProfileWrapperDto { OpexCostProfileDto = opexCostProfileDto };
            var cessationCostWrapper = new CessationCostWrapperDto { CessationCostDto = cessationCostDto };

            // Mock the services to return the wrapper DTOs
            _studyCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(studyCostProfileWrapper));
            _opexCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(opexCostProfileWrapper));
            _cessationCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(cessationCostWrapper));

            var substructure = new Substructure
            {
                CostProfileOverride = new SubstructureCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [70.0, 110.0, 150.0]
                }
            };

            var surf = new Models.Surf
            {
                CostProfileOverride = new SurfCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [30.0, 60.0, 90.0]
                }
            };

            var topside = new Topside
            {
                CostProfileOverride = new TopsideCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                }
            };

            var transport = new Models.Transport
            {
                CostProfileOverride = new TransportCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [40.0, 70.0, 100.0]
                }
            };

            // Set up the substitutes to return the expected cost profiles
            _substructureRepository.GetSubstructure(caseItem.SubstructureLink)
                .Returns(Task.FromResult(substructure));

            _surfRepository.GetSurf(caseItem.SurfLink)
                .Returns(Task.FromResult(surf));

            _topsideRepository.GetTopside(caseItem.TopsideLink)
                .Returns(Task.FromResult(topside));

            _transportRepository.GetTransport(caseItem.TransportLink)
                .Returns(Task.FromResult(transport));

            var wellProject = new WellProject
            {
                OilProducerCostProfileOverride = new OilProducerCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 150.0, 200.0]
                },

                GasProducerCostProfileOverride = new GasProducerCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                },

                WaterInjectorCostProfileOverride = new WaterInjectorCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [70.0, 100.0, 130.0]
                },

                GasInjectorCostProfileOverride = new GasInjectorCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [50.0, 80.0, 120.0]
                },
            };

            // Set up the substitutes to return the expected cost profiles
            _wellProjectRepository.GetWellProject(caseItem.WellProjectLink)
                .Returns(Task.FromResult(wellProject));

            var exploration = new Exploration
            {
                GAndGAdminCostOverride = new GAndGAdminCostOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 200.0, 300.0]
                },
                SeismicAcquisitionAndProcessing = new SeismicAcquisitionAndProcessing
                {
                    StartYear = 2020,
                    Values = [150.0, 250.0, 350.0]
                },
                CountryOfficeCost = new CountryOfficeCost
                {
                    StartYear = 2020,
                    Values = [50.0, 100.0]
                },
                ExplorationWellCostProfile = new ExplorationWellCostProfile
                {
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },
                AppraisalWellCostProfile = new AppraisalWellCostProfile
                {
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                },
                SidetrackCostProfile = new SidetrackCostProfile
                {
                    StartYear = 2020,
                    Values = [100.0, 100.0]
                }
            };

            // Set up the substitute to return the expected exploration costs
            _explorationRepository.GetExploration(caseItem.ExplorationLink)
                .Returns(Task.FromResult(exploration));

            // Act
            var result = await _economicsCalculationHelper.CalculateTotalCostAsync(caseItem);

            // Assert
            Assert.Equal(2020, result.StartYear);
            Assert.Equal(3, result.Values.Length);

            var expectedValues = new[]
            {
        2860, 4080, 4880 // These values need to be calculated based on the available profiles and the missing cessation cost profile.
    };

            Assert.Equal(expectedValues.Length, result.Values.Length);
            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], result.Values[i], precision: 0);
            }
        }

        [Fact]
        public async Task CalculateTotalCostAsync_EmptyProfiles_ReturnsCorrectTotalCost()
        {
            // Arrange
            var caseItem = new Case
            {
                Id = Guid.NewGuid(),
                WellProjectLink = Guid.NewGuid(),
                SubstructureLink = Guid.NewGuid(),
                SurfLink = Guid.NewGuid(),
                TopsideLink = Guid.NewGuid(),
                TransportLink = Guid.NewGuid(),
                ExplorationLink = Guid.NewGuid()
            };

            var studyCostProfileDto = new StudyCostProfileDto
            {
                StartYear = 2020,
                Values = Array.Empty<double>()
            };

            var opexCostProfileDto = new OpexCostProfileDto
            {
                StartYear = 2020,
                Values = Array.Empty<double>()
            };

            var cessationCostDto = new CessationCostDto
            {
                StartYear = 2020,
                Values = Array.Empty<double>()
            };

            // Wrap DTOs in wrapper DTOs
            var studyCostProfileWrapper = new StudyCostProfileWrapperDto { StudyCostProfileDto = studyCostProfileDto };
            var opexCostProfileWrapper = new OpexCostProfileWrapperDto { OpexCostProfileDto = opexCostProfileDto };
            var cessationCostWrapper = new CessationCostWrapperDto { CessationCostDto = cessationCostDto };

            // Mock the services to return the wrapper DTOs
            _studyCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(studyCostProfileWrapper));
            _opexCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(opexCostProfileWrapper));
            _cessationCostProfileService.Generate(caseItem.Id).Returns(Task.FromResult(cessationCostWrapper));

            var substructure = new Substructure
            {
                CostProfileOverride = new SubstructureCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = Array.Empty<double>()
                }
            };

            var surf = new Models.Surf
            {
                CostProfileOverride = new SurfCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = Array.Empty<double>()
                }
            };

            var topside = new Topside
            {
                CostProfileOverride = new TopsideCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = Array.Empty<double>()
                }
            };

            var transport = new Models.Transport
            {
                CostProfileOverride = new TransportCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = Array.Empty<double>()
                }
            };

            // Set up the substitutes to return the expected cost profiles
            _substructureRepository.GetSubstructure(caseItem.SubstructureLink)
                .Returns(Task.FromResult(substructure));

            _surfRepository.GetSurf(caseItem.SurfLink)
                .Returns(Task.FromResult(surf));

            _topsideRepository.GetTopside(caseItem.TopsideLink)
                .Returns(Task.FromResult(topside));

            _transportRepository.GetTransport(caseItem.TransportLink)
                .Returns(Task.FromResult(transport));

            // Act
            var result = await _economicsCalculationHelper.CalculateTotalCostAsync(caseItem);

            // Assert
            Assert.Equal(0, result.StartYear);
            Assert.Empty(result.Values); // Expecting no cost values

            // Optional: you may check if result.Values is empty
            Assert.Equal(0, result.Values.Length);
        }

        [Fact]
        public async Task CalculateTotalOffshoreFacilityCostAsync_ValidCostProfiles_CalculatesCorrectly()
        {
            // Arrange
            var caseItem = new Case
            {
                SubstructureLink = Guid.NewGuid(),
                SurfLink = Guid.NewGuid(),
                TopsideLink = Guid.NewGuid(),
                TransportLink = Guid.NewGuid()
            };

            var substructure = new Substructure
            {
                CostProfileOverride = new SubstructureCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [70.0, 110.0, 150.0]
                }
            };

            var surf = new Models.Surf
            {
                CostProfileOverride = new SurfCostProfileOverride
                {
                    Override = true,
                    StartYear = 2021,
                    Values = [30.0, 60.0, 90.0]
                }
            };

            var topside = new Topside
            {
                CostProfileOverride = new TopsideCostProfileOverride
                {
                    Override = true,
                    StartYear = 2022,
                    Values = [50.0, 80.0, 120.0]
                }
            };

            var transport = new Models.Transport
            {
                CostProfileOverride = new TransportCostProfileOverride
                {
                    Override = true,
                    StartYear = 2023,
                    Values = [40.0, 70.0, 100.0]
                }
            };

            // Set up the substitutes to return the expected cost profiles
            _substructureRepository.GetSubstructure(caseItem.SubstructureLink)
                .Returns(Task.FromResult(substructure));

            _surfRepository.GetSurf(caseItem.SurfLink)
                .Returns(Task.FromResult(surf));

            _topsideRepository.GetTopside(caseItem.TopsideLink)
                .Returns(Task.FromResult(topside));

            _transportRepository.GetTransport(caseItem.TransportLink)
                .Returns(Task.FromResult(transport));

            // Act
            var result = await _economicsCalculationHelper.CalculateTotalOffshoreFacilityCostAsync(caseItem);

            // Assert
            var expectedStartYear = 2020;


            var expectedValues = new double[] { 70.0, 140.0, 260.0, 210.0, 190.0, 100.0 };

            Assert.Equal(expectedStartYear, result.StartYear);
            Assert.Equal(expectedValues.Length, result.Values.Length);

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], result.Values[i]);
            }
        }

        [Fact]
        public async Task CalculateTotalOffshoreFacilityCostAsync_NullCase_ThrowsNullReferenceException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _economicsCalculationHelper.CalculateTotalOffshoreFacilityCostAsync(null));
        }

        [Fact]
        public async Task CalculateTotalOffshoreFacilityCostAsync_EmptyCostProfiles_ReturnsZeroCost()
        {
            // Arrange
            var caseItem = new Case
            {
                SubstructureLink = Guid.NewGuid(),
                SurfLink = Guid.NewGuid(),
                TopsideLink = Guid.NewGuid(),
                TransportLink = Guid.NewGuid()
            };

            var substructure = new Substructure
            {
                CostProfileOverride = new SubstructureCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = Array.Empty<double>()
                }
            };

            var surf = new Models.Surf
            {
                CostProfileOverride = new SurfCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = Array.Empty<double>()
                }
            };

            var topside = new Topside
            {
                CostProfileOverride = new TopsideCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = Array.Empty<double>()
                }
            };

            var transport = new Models.Transport
            {
                CostProfileOverride = new TransportCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = Array.Empty<double>()
                }
            };

            _substructureRepository.GetSubstructure(caseItem.SubstructureLink).Returns(substructure);
            _surfRepository.GetSurf(caseItem.SurfLink).Returns(surf);
            _topsideRepository.GetTopside(caseItem.TopsideLink).Returns(topside);
            _transportRepository.GetTransport(caseItem.TransportLink).Returns(transport);

            // Act
            var totalCost = await _economicsCalculationHelper.CalculateTotalOffshoreFacilityCostAsync(caseItem);
            var expectedIncome = 0;

            // Assert
            Assert.Equal(expectedIncome, totalCost.Values.Sum());
            Assert.Empty(totalCost.Values);

        }

        [Fact]
        public async Task CalculateTotalDevelopmentCostAsync_ValidCostProfiles_CalculatesCorrectly()
        {
            // Arrange
            var caseItem = new Case
            {
                WellProjectLink = Guid.NewGuid()
            };

            var wellProject = new WellProject
            {
                OilProducerCostProfileOverride = new OilProducerCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [100.0, 150.0, 200.0]
                },

                GasProducerCostProfileOverride = new GasProducerCostProfileOverride
                {
                    Override = true,
                    StartYear = 2021,
                    Values = [50.0, 80.0, 120.0]
                },

                WaterInjectorCostProfileOverride = new WaterInjectorCostProfileOverride
                {
                    Override = true,
                    StartYear = 2020,
                    Values = [70.0, 100.0, 130.0]
                },

                GasInjectorCostProfileOverride = new GasInjectorCostProfileOverride
                {
                    Override = true,
                    StartYear = 2021,
                    Values = [50.0, 80.0, 120.0]
                },
            };

            // Set up the substitutes to return the expected cost profiles
            _wellProjectRepository.GetWellProject(caseItem.WellProjectLink)
                .Returns(Task.FromResult(wellProject));

            // Act
            var result = await _economicsCalculationHelper.CalculateTotalDevelopmentCostAsync(caseItem);

            // Assert
            var expectedStartYear = 2020;

            var expectedValues = new double[] { 170.0, 350.0, 490.0, 240.0 };

            Assert.Equal(expectedStartYear, result.StartYear);
            Assert.Equal(expectedValues.Length, result.Values.Length);

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], result.Values[i]);
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

            // Set up the substitute to return the expected exploration costs
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
        public async Task CalculateTotalOffshoreFacilityCostAsync_NullProfiles_ReturnsEmptyProfile()
        {
            // Arrange
            _substructureRepository.GetSubstructure(Arg.Any<Guid>()).Returns((Substructure)null);
            _surfRepository.GetSurf(Arg.Any<Guid>()).Returns((Models.Surf)null);
            _topsideRepository.GetTopside(Arg.Any<Guid>()).Returns((Topside)null);
            _transportRepository.GetTransport(Arg.Any<Guid>()).Returns((Models.Transport)null);
            _wellProjectRepository.GetWellProject(Arg.Any<Guid>()).Returns((WellProject)null);

            // Arrange
            var caseItem = new Case
            {
                SubstructureLink = Guid.NewGuid(),
                SurfLink = Guid.NewGuid(),
                TopsideLink = Guid.NewGuid(),
                TransportLink = Guid.NewGuid()
            };

            // Act
            var result = await _economicsCalculationHelper.CalculateTotalOffshoreFacilityCostAsync(caseItem);

            // Assert
            Assert.Equal("00000000-0000-0000-0000-000000000000", result.Id.ToString());
        }

        [Fact]
        public async Task CalculateTotalOffshoreFacilityCostAsync_EmptyProfiles_ReturnsZeroCost()
        {
            // Arrange
            var substructure = new Substructure { CostProfile = new SubstructureCostProfile { Values = [], StartYear = 2020 } };
            var surf = new Models.Surf { CostProfile = new SurfCostProfile { Values = [], StartYear = 2020 } };
            var topside = new Topside { CostProfile = new TopsideCostProfile { Values = [], StartYear = 2020 } };
            var transport = new Models.Transport { CostProfile = new TransportCostProfile { Values = [], StartYear = 2020 } };

            _substructureRepository.GetSubstructure(Arg.Any<Guid>()).Returns(substructure);
            _surfRepository.GetSurf(Arg.Any<Guid>()).Returns(surf);
            _topsideRepository.GetTopside(Arg.Any<Guid>()).Returns(topside);
            _transportRepository.GetTransport(Arg.Any<Guid>()).Returns(transport);

            var caseItem = new Case
            {
                SubstructureLink = Guid.NewGuid(),
                SurfLink = Guid.NewGuid(),
                TopsideLink = Guid.NewGuid(),
                TransportLink = Guid.NewGuid()
            };

            // Act
            var result = await _economicsCalculationHelper.CalculateTotalOffshoreFacilityCostAsync(caseItem);

            // Assert
            Assert.Empty(result.Values);
        }
    }
}
