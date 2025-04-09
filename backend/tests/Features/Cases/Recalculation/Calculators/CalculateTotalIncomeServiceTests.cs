using api.Features.Profiles;
using api.Features.Recalculation.Production;
using api.Features.Recalculation.RevenuesAndCashflow;
using api.Models;
using api.Models.Enums;

using Xunit;

using static api.Features.Profiles.CalculationConstants;

namespace tests.Features.Cases.Recalculation.Calculators;

public class CalculateTotalIncomeServiceTests
{
    [Fact]
    public void CalculateIncome_ValidInput_ReturnsCorrectIncome()
    {
        // Arrange
        var caseId = Guid.NewGuid();
        var oilPriceUsd = 75;
        var gasPriceNok = 3;
        var currencyRate = 10;

        var project = new Project
        {
            Id = Guid.NewGuid(),
            OilPriceUsd = oilPriceUsd,
            GasPriceNok = 3,
            ExchangeRateUsdToNok = 10,
            Currency = Currency.Usd
        };

        var topside = new Topside
        {
            FuelConsumption = 0.019,
            OilCapacity = 555,
            Co2ShareOilProfile = 0.173,
            Co2OnMaxOilProfile = 0.208,
            GasCapacity = 0.96,
            Co2ShareGasProfile = 0.827,
            Co2OnMaxGasProfile = 0.057,
            WaterInjectionCapacity = 0,
            Co2ShareWaterInjectionProfile = 0,
            Co2OnMaxWaterInjectionProfile = 0,

            DryWeight = 0,
            ArtificialLift = ArtificialLift.NoArtificialLift,
            Maturity = Maturity.A,
            FlaredGas = 0,
            ProducerCount = 0,
            GasInjectorCount = 0,
            WaterInjectorCount = 0,
            CostYear = 0,
            ProspVersion = null,
            Source = Source.ConceptApp,
            ApprovedBy = "",
            FacilityOpex = 0,
            PeakElectricityImported = 0
        };

        var drainageStrategy = new DrainageStrategy
        {
            GasSolution = GasSolution.Export,
            NglYield = 0,
            CondensateYield = 0,
            GasShrinkageFactor = 100,
            ProducerCount = 0,
            GasInjectorCount = 0,
            WaterInjectorCount = 0,
            ArtificialLift = ArtificialLift.NoArtificialLift,
        };

        var caseItem = new Case
        {
            Id = caseId,
            Project = project,
            ProjectId = project.Id,
            DrainageStrategyId = Guid.NewGuid(),
            DrainageStrategy = drainageStrategy,
            Topside = topside,
            TimeSeriesProfiles =
            [
                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileOil,
                    StartYear = 2020,
                    Values = [1000000.0, 2000000.0, 3000000.0] // SM³
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileOil,
                    StartYear = 2020,
                    Values = [1000000.0, 2000000.0] // SM³
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.ProductionProfileGas,
                    StartYear = 2020,
                    Values = [1000000000.0, 2000000000.0, 3000000000.0] // SM³
                },

                new TimeSeriesProfile
                {
                    ProfileType = ProfileTypes.AdditionalProductionProfileGas,
                    StartYear = 2020,
                    Values = [1000000000.0, 2000000000.0] // SM³
                }
            ]
        };

        CalculateTotalOilIncomeService.RunCalculation(caseItem);
        var expectedFirstYearOilIncome = 2000000.0 * oilPriceUsd * BarrelsPerCubicMeter;
        var expectedSecondYearOilIncome = 4000000.0 * oilPriceUsd * BarrelsPerCubicMeter;
        var expectedThirdYearOilIncome = 3000000.0 * oilPriceUsd * BarrelsPerCubicMeter;

        var calculatedTotalOilIncomeCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalOilIncomeCostProfile);
        Assert.NotNull(calculatedTotalOilIncomeCostProfile);
        Assert.Equal(2020, calculatedTotalOilIncomeCostProfile.StartYear);
        Assert.Equal(3, calculatedTotalOilIncomeCostProfile.Values.Length);
        Assert.Equal(expectedFirstYearOilIncome, calculatedTotalOilIncomeCostProfile.Values[0], precision: 0);
        Assert.Equal(expectedSecondYearOilIncome, calculatedTotalOilIncomeCostProfile.Values[1], precision: 0);
        Assert.Equal(expectedThirdYearOilIncome, calculatedTotalOilIncomeCostProfile.Values[2], precision: 0);

        NetSaleGasProfileService.RunCalculation(caseItem);
        const double expectedFirstYearGasIncome = 2000000000.0;
        const double expectedSecondYearGasIncome = 4000000000.0;
        const double expectedThirdYearGasIncome = 3000000000.0;

        var netSalesGas = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas);
        Assert.NotNull(netSalesGas);
        Assert.Equal(2020, netSalesGas.StartYear);
        Assert.Equal(3, netSalesGas.Values.Length);
        Assert.Equal(expectedFirstYearGasIncome, netSalesGas.Values[0], precision: 0);
        Assert.Equal(expectedSecondYearGasIncome, netSalesGas.Values[1], precision: 0);
        Assert.Equal(expectedThirdYearGasIncome, netSalesGas.Values[2], precision: 0);

        CalculateTotalIncomeService.RunCalculation(caseItem);
        var expectedFirstYearIncome = (2000000.0 * oilPriceUsd * BarrelsPerCubicMeter * currencyRate + 2000000000.0 * gasPriceNok) / currencyRate;
        var expectedSecondYearIncome = (4000000.0 * oilPriceUsd * BarrelsPerCubicMeter * currencyRate + 4000000000.0 * gasPriceNok) / currencyRate;
        var expectedThirdYearIncome = (3000000.0 * oilPriceUsd * BarrelsPerCubicMeter * currencyRate + 3000000000.0 * gasPriceNok) / currencyRate;

        var calculatedTotalIncomeCostProfile = caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfile);
        Assert.NotNull(calculatedTotalIncomeCostProfile);
        Assert.Equal(2020, calculatedTotalIncomeCostProfile.StartYear);
        Assert.Equal(3, calculatedTotalIncomeCostProfile.Values.Length);
        Assert.Equal(expectedFirstYearIncome, calculatedTotalIncomeCostProfile.Values[0], precision: 0);
        Assert.Equal(expectedSecondYearIncome, calculatedTotalIncomeCostProfile.Values[1], precision: 0);
        Assert.Equal(expectedThirdYearIncome, calculatedTotalIncomeCostProfile.Values[2], precision: 0);
    }
}
