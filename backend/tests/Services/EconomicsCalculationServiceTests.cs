using System.Linq.Expressions;

using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.Assets.CaseAssets.Explorations.Services;
using api.Features.Assets.CaseAssets.Substructures.Services;
using api.Features.Assets.CaseAssets.Surfs.Services;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Features.Assets.CaseAssets.Transports.Services;
using api.Features.Assets.CaseAssets.WellProjects.Services;
using api.Features.CaseProfiles.Services;
using api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;
using api.Features.Cases.Recalculation.Calculators.CalculateNpv;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;
using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Features.Cases.Recalculation.Types.CessationCostProfile;
using api.Features.Cases.Recalculation.Types.OpexCostProfile;
using api.Features.Cases.Recalculation.Types.StudyCostProfile;
using api.Models;

using NSubstitute;

using Xunit;

namespace tests.Services;

public class EconomicsCalculationServiceTests
{
    private readonly IExplorationService _explorationService;
    private readonly ISubstructureService _substructureService;
    private readonly ISurfService _surfService;
    private readonly ITopsideService _topsideService;
    private readonly ITransportService _transportService;
    private readonly IOnshorePowerSupplyService _onshorePowerSupplyService;
    private readonly IWellProjectService _wellProjectService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IStudyCostProfileService _studyCostProfileService;
    private readonly IOpexCostProfileService _opexCostProfileService;
    private readonly ICessationCostProfileService _cessationCostProfileService;
    private readonly ICaseService _caseService;
    private readonly CalculateTotalIncomeService _calculateTotalIncomeService;
    private readonly CalculateBreakEvenOilPriceService _calculateBreakEvenOilPriceService;
    private readonly CalculateTotalCostService _calculateTotalCostService;
    private readonly CalculateNpvService _calculateNpvService;

    public EconomicsCalculationServiceTests()
    {
        _caseService = Substitute.For<ICaseService>();
        _drainageStrategyService = Substitute.For<IDrainageStrategyService>();
        _substructureService = Substitute.For<ISubstructureService>();
        _surfService = Substitute.For<ISurfService>();
        _topsideService = Substitute.For<ITopsideService>();
        _transportService = Substitute.For<ITransportService>();
        _onshorePowerSupplyService = Substitute.For<IOnshorePowerSupplyService>();
        _explorationService = Substitute.For<IExplorationService>();
        _wellProjectService = Substitute.For<IWellProjectService>();
        _onshorePowerSupplyService = Substitute.For<IOnshorePowerSupplyService>();
        _cessationCostProfileService = Substitute.For<ICessationCostProfileService>();
        _opexCostProfileService = Substitute.For<IOpexCostProfileService>();
        _studyCostProfileService = Substitute.For<IStudyCostProfileService>();

        _calculateTotalIncomeService = new CalculateTotalIncomeService(
            _caseService,
            _drainageStrategyService
        );

        _calculateBreakEvenOilPriceService = new CalculateBreakEvenOilPriceService(
            _caseService,
            _drainageStrategyService
        );
        _calculateTotalCostService = new CalculateTotalCostService(
            _caseService,
            _substructureService,
            _surfService,
            _topsideService,
            _transportService,
            _onshorePowerSupplyService,
            _wellProjectService,
            _explorationService
        );
        _calculateNpvService = new CalculateNpvService(_caseService);
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
        await _calculateTotalIncomeService.CalculateTotalIncome(caseId);

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
        await Assert.ThrowsAsync<NullReferenceException>(async () => await _calculateTotalIncomeService.CalculateTotalIncome(caseId));
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
        await _calculateTotalIncomeService.CalculateTotalIncome(caseId);

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
            OnshorePowerSupplyLink = Guid.NewGuid(),
            ExplorationLink = Guid.NewGuid(),
            TotalOtherStudiesCostProfile = new TotalOtherStudiesCostProfile
            {
                StartYear = 2020,
                Values = new[] { 1000.0, 1500.0, 2000.0 }
            },
            OnshoreRelatedOPEXCostProfile = new OnshoreRelatedOPEXCostProfile
            {
                StartYear = 2020,
                Values = new[] { 500.0, 600.0, 700.0 }
            },
            CessationWellsCost = new CessationWellsCost
            {
                StartYear = 2020,
                Values = new[] { 300.0, 400.0, 500.0 }
            }
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

        var surf = new Surf
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

        var transport = new Transport
        {
            CostProfileOverride = new TransportCostProfileOverride
            {
                Override = true,
                StartYear = 2020,
                Values = new[] { 50.0, 70.0, 100.0 }
            }
        };

        var onshorePowerSupply = new OnshorePowerSupply
        {
            Name = "TestName",
            CostProfileOverride = new OnshorePowerSupplyCostProfileOverride
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


        _substructureService.GetSubstructureWithIncludes(caseItem.SubstructureLink, Arg.Any<Expression<Func<Substructure, object>>[]>())
            .Returns(Task.FromResult(substructure));

        _surfService.GetSurfWithIncludes(caseItem.SurfLink, Arg.Any<Expression<Func<Surf, object>>[]>())
            .Returns(Task.FromResult(surf));

        _topsideService.GetTopsideWithIncludes(caseItem.TopsideLink, Arg.Any<Expression<Func<Topside, object>>[]>())
            .Returns(Task.FromResult(topside));

        _transportService.GetTransportWithIncludes(caseItem.TransportLink, Arg.Any<Expression<Func<Transport, object>>[]>())
            .Returns(Task.FromResult(transport));

        _onshorePowerSupplyService.GetOnshorePowerSupplyWithIncludes(caseItem.OnshorePowerSupplyLink, Arg.Any<Expression<Func<OnshorePowerSupply, object>>[]>())
            .Returns(Task.FromResult(onshorePowerSupply));

        _wellProjectService.GetWellProjectWithIncludes(caseItem.WellProjectLink, Arg.Any<Expression<Func<WellProject, object>>[]>())
            .Returns(Task.FromResult(wellProject));

        _explorationService.GetExplorationWithIncludes(caseItem.ExplorationLink, Arg.Any<Expression<Func<Exploration, object>>[]>())
            .Returns(Task.FromResult(exploration));

        _caseService.GetCaseWithIncludes(caseId, Arg.Any<Expression<Func<Case, object>>[]>())
            .Returns(caseItem);

        // Act
        await _calculateTotalCostService.CalculateTotalCost(caseId);

        // Assert
        Assert.Equal(2020, caseItem.CalculatedTotalCostCostProfile!.StartYear);
        Assert.Equal(3, caseItem.CalculatedTotalCostCostProfile.Values.Length);

        var expectedValues = new[] { 2950.0, 4150.0, 4980.0 };
        for (int i = 0; i < expectedValues.Length; i++)
        {
            Assert.Equal(expectedValues[i], caseItem.CalculatedTotalCostCostProfile.Values[i]);
        }
    }

    [Fact]
    public void CalculateTotalExplorationCostAsync_ValidInput_ReturnsCorrectTotalExplorationCost()
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

        _explorationService.GetExplorationWithIncludes(caseItem.ExplorationLink)
            .Returns(Task.FromResult(exploration));

        // Act
        var result = CalculateTotalCostService.CalculateTotalExplorationCost(exploration);

        // Assert
        var expectedStartYear = 2020;
        var expectedValues = new[] { 55.0, 175.0, 165.0, 35.0 };

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

        var expectedValues = new[] { 300.0, 400.0, 500.0, -500.0 };

        // Act
        var result = EconomicsHelper.CalculateCashFlow(income, totalCost);

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
        var discountedVolume = EconomicsHelper.CalculateDiscountedVolume(values, discountRate, startIndex);

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

        _caseService.GetCaseWithIncludes(caseId, Arg.Any<Expression<Func<Case, object>>[]>())
            .Returns(caseItem);

        await _calculateNpvService.CalculateNpv(caseId);

        var actualNpvValue = 1081.62;
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
        await _calculateBreakEvenOilPriceService.CalculateBreakEvenOilPrice(caseId);

        // Assert
        var expectedBreakEvenPrice = 26.29;
        Assert.Equal(expectedBreakEvenPrice, caseItem.BreakEven, precision: 1);
    }
}
