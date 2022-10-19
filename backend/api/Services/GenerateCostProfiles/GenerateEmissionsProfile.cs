using api.Adapters;
using api.Dtos;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateEmissionsProfile
{
    private const double Co2EmissionRemovedFromGas = 0;
    private const double FlaredGasPerProducedVolume = 1.321;
    private const double Co2EmissionFromFuelGas = 2.34;
    private const double Co2EmissionFromFlaredGas = 3.74;
    private const double Co2Vented = 1.96;
    private const int AvereageDevelopmentWellDrillingDays = 50;
    private const int DailyEmissionFromDrillingRig = 100;
    private const int DaysInLeapYear = 366;
    private const int DaysInYear = 365;
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ProjectService _projectService;
    private readonly TopsideService _topsideService;
    private readonly WellProjectService _wellProjectService;

    public GenerateEmissionsProfile(IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _wellProjectService = serviceProvider.GetRequiredService<WellProjectService>();
    }

    public NetSalesGasDto GenerateNetSaleGas(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProject(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var fuelConsumptions = CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = CalculateFlaring(drainageStrategy);
        var losses = CalculateLosses(drainageStrategy);
        var calculateNetSaleGas = CalculateNetSaleGas(drainageStrategy, fuelConsumptions, flarings, losses);

        var netSaleGas = new NetSalesGas
        {
            StartYear = calculateNetSaleGas.StartYear,
            Values = calculateNetSaleGas.Values
        };

        var dto = DrainageStrategyDtoAdapter.Convert(netSaleGas, project.PhysicalUnit);
        return dto;
    }

    public FuelFlaringAndLossesDto GenerateFuelFlaringAndLosses(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProject(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var fuelConsumptions = CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = CalculateFlaring(drainageStrategy);
        var losses = CalculateLosses(drainageStrategy);

        var totalProfile =
            TimeSeriesCost.MergeCostProfiles(TimeSeriesCost.MergeCostProfiles(fuelConsumptions, flarings), losses);


        var fuelFlaringLosses = new FuelFlaringAndLosses
        {
            StartYear = totalProfile.StartYear,
            Values = totalProfile.Values
        };

        var dto = DrainageStrategyDtoAdapter.Convert(fuelFlaringLosses, project.PhysicalUnit);
        return dto;
    }

    public Co2EmissionsDto GenerateCo2Emissions(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProject(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var wellProject = _wellProjectService.GetWellProject(caseItem.WellProjectLink);
        var fuelConsumptions = CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);

        var fuelConsumptionWithCfg = Array.Empty<double>();
        for (var i = 0; i <= fuelConsumptions.Values.Length; i++)
        {
            var calculateFuelConsumptionWithCfg = fuelConsumptions.Values[i] * Co2EmissionFromFuelGas;
            fuelConsumptionWithCfg[i] = calculateFuelConsumptionWithCfg;
        }

        var fuelConsumptionsProfile = new TimeSeriesVolume
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptionWithCfg
        };


        var flarings = CalculateFlaring(drainageStrategy);
        var flaringsWithCFF = Array.Empty<double>();
        for (var i = 0; i < flarings.Values.Length; i++)
        {
            var calculatedFlaringsWithCFF = flarings.Values[i] * Co2EmissionFromFlaredGas;
            flaringsWithCFF[i] = calculatedFlaringsWithCFF;
        }

        var flaringsProfile = new TimeSeriesVolume
        {
            StartYear = flarings.StartYear,
            Values = flaringsWithCFF
        };

        var lossesWithCV = Array.Empty<double>();
        var losses = CalculateLosses(drainageStrategy);
        for (var i = 0; i < losses.Values.Length; i++)
        {
            var calculatedLossesWithCV = losses.Values[i] * Co2Vented;
            lossesWithCV[i] = calculatedLossesWithCV;
        }

        var lossesProfile = new TimeSeriesVolume
        {
            StartYear = losses.StartYear,
            Values = lossesWithCV
        };

        var drillingEmissionsProfile = CalculateDrillingEmissions(drainageStrategy, wellProject);

        var totalProfile =
            TimeSeriesCost.MergeCostProfiles(TimeSeriesCost.MergeCostProfiles(
                TimeSeriesCost.MergeCostProfiles(fuelConsumptionsProfile, flaringsProfile),
                lossesProfile), drillingEmissionsProfile);
        var co2Emission = new Co2Emissions
        {
            StartYear = totalProfile.StartYear,
            Values = totalProfile.Values
        };

        var dto = DrainageStrategyDtoAdapter.Convert(co2Emission, project.PhysicalUnit);
        return dto;
    }

    private static TimeSeriesVolume CalculateDrillingEmissions(DrainageStrategy drainageStrategy,
        WellProject wellProject)
    {
        var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory))
            .ToList();
        if (linkedWells == null)
        {
            return new TimeSeriesVolume();
        }

        var wellDrillingSchedules = new TimeSeries<double>();
        foreach (var linkedWell in linkedWells)
        {
            if (linkedWell.DrillingSchedule == null)
            {
                continue;
            }

            var timeSeries = new TimeSeries<double>
            {
                StartYear = linkedWell.DrillingSchedule.StartYear,
                Values = linkedWell.DrillingSchedule.Values.Select(v => (double)v).ToArray()
            };
            wellDrillingSchedules = TimeSeriesCost.MergeCostProfiles(wellDrillingSchedules, timeSeries);
        }

        var sumOfWellsDrilledInYear = Array.Empty<double>();
        for (var i = 0; i < wellDrillingSchedules.Values.Length; i++)
        {
            var calculatedDrillingEmissions = wellDrillingSchedules.Values[i] *
                AvereageDevelopmentWellDrillingDays * DailyEmissionFromDrillingRig / 1000000;
            sumOfWellsDrilledInYear[i] = calculatedDrillingEmissions;
        }

        var drillingEmission = new ProductionProfileGas
        {
            StartYear = drainageStrategy.ProductionProfileGas.StartYear,
            Values = sumOfWellsDrilledInYear
        };

        return drillingEmission;
    }

    private static TimeSeries<double> CalculateNetSaleGas(DrainageStrategy drainageStrategy,
        TimeSeries<double> fuelConsumption, TimeSeries<double> flarings, TimeSeries<double> losses)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return null!;
        }

        var fuelFlaringLosses =
            TimeSeriesCost.MergeCostProfiles(TimeSeriesCost.MergeCostProfiles(fuelConsumption, flarings), losses);

        var negativeFuelFlaringLosses = new TimeSeriesVolume
        {
            StartYear = fuelFlaringLosses.StartYear,
            Values = fuelFlaringLosses.Values.Select(x => x * -1).ToArray()
        };

        return TimeSeriesCost.MergeCostProfiles(drainageStrategy.ProductionProfileGas, negativeFuelFlaringLosses);
    }

    private TimeSeries<double> CalculateFlaring(DrainageStrategy drainageStrategy)
    {
        var oilValues = Array.Empty<double>();
        var gasValues = Array.Empty<double>();
        var oilRatesInFlaring = CalculatedOilRatesInFlaring(drainageStrategy, oilValues);
        var gasRatesInFlaring = CalculatedGasRatesInFlaring(drainageStrategy, gasValues);
        return TimeSeriesCost.MergeCostProfiles(oilRatesInFlaring, gasRatesInFlaring);
    }

    private static TimeSeriesVolume CalculatedOilRatesInFlaring(DrainageStrategy drainageStrategy, double[] oilValues)
    {
        if (drainageStrategy.ProductionProfileOil == null)
        {
            return null!;
        }

        var oilRates = drainageStrategy.ProductionProfileOil.Values;
        for (var i = 0; i <= oilRates.Length; i++)
        {
            var calculatedOil = oilRates[i] * FlaredGasPerProducedVolume / 1000;
            oilValues[i] = calculatedOil;
        }

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileOil.StartYear,
            Values = oilValues
        };
    }

    private static TimeSeriesVolume CalculatedGasRatesInFlaring(DrainageStrategy drainageStrategy, double[] gasValues)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return null!;
        }

        var gasRates = drainageStrategy.ProductionProfileGas.Values;
        for (var i = 0; i <= gasRates.Length; i++)
        {
            var calculatedOil = gasRates[i] * FlaredGasPerProducedVolume / 1000;
            gasValues[i] = calculatedOil;
        }

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileGas.StartYear,
            Values = gasValues
        };
    }

    private static TimeSeries<double> CalculateLosses(DrainageStrategy drainageStrategy)
    {
        var gasValues = Array.Empty<double>();
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return null!;
        }

        var gasRates = drainageStrategy.ProductionProfileGas.Values;
        for (var i = 0; i <= gasRates.Length; i++)
        {
            var calculatedOil = gasRates[i] * Co2EmissionRemovedFromGas;
            gasValues[i] = calculatedOil;
        }

        return new TimeSeriesVolume
        {
            StartYear = drainageStrategy.ProductionProfileGas.StartYear,
            Values = gasValues
        };
    }

    private TimeSeries<double> CalculateTotalFuelConsumptions(Case caseItem, Topside topside,
        DrainageStrategy drainageStrategy)
    {
        var totalUseOfPower = CalculateTotalUseOfPower(topside, drainageStrategy);
        var productionEfficiency = caseItem.FacilitiesAvailability;
        var fuelGasConsumptionMax = topside.FuelConsumption; //Confirm with A if this is the correct value

        var valuesList = new List<double>();
        for (var i = totalUseOfPower.StartYear; i < totalUseOfPower.StartYear + totalUseOfPower.Values.Length; i++)
        {
            var year = caseItem.DG4Date.Year + i;
            var calendarDays = DateTime.IsLeapYear(year) ? DaysInLeapYear : DaysInYear;

            var fuelGasConsumption = productionEfficiency * fuelGasConsumptionMax * calendarDays / 1000;

            valuesList.Add(fuelGasConsumption);
        }

        var newTimeSeries = new TimeSeries<double>
        {
            StartYear = totalUseOfPower.StartYear,
            Values = valuesList.ToArray()
        };


        return TimeSeriesCost.MergeCostProfiles(totalUseOfPower, newTimeSeries);
    }

    private TimeSeries<double> CalculateTotalUseOfPower(Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerOil = CalculateTotalUseOfPowerOil(topside, drainageStrategy);
        var totalUseOfPowerGas = CalculateTotalUseOfPowerGas(topside, drainageStrategy);
        var totalUseOfPowerWaterInjection = CalculateTotalUseOfPowerWaterInjection(topside, drainageStrategy);

        var totalUseOfPower = TimeSeriesCost.MergeCostProfiles(
            TimeSeriesCost.MergeCostProfiles(totalUseOfPowerOil, totalUseOfPowerGas), totalUseOfPowerWaterInjection);

        return totalUseOfPower;
    }

    private TimeSeriesVolume CalculateTotalUseOfPowerWaterInjection(Topside topside,
        DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerWaterInjection = new TimeSeriesVolume();
        var co2ShareWaterInjectionProfile = topside.CO2ShareWaterInjectionProfile;
        var co2OnMaxWaterInjection = topside.CO2OnMaxWaterInjectionProfile;

        var waterInjectionValues = Array.Empty<double>();


        if (drainageStrategy.ProductionProfileWaterInjection == null)
        {
            return totalUseOfPowerWaterInjection;
        }

        var waterInjectionRates = drainageStrategy.ProductionProfileWaterInjection.Values;

        for (var i = 0; i <= waterInjectionRates.Length; i++)
        {
            var calculatedPowerWaterInjection = co2ShareWaterInjectionProfile * co2OnMaxWaterInjection +
                                                waterInjectionRates[i] *
                                                co2ShareWaterInjectionProfile * (1 - co2OnMaxWaterInjection);
            waterInjectionValues[i] = calculatedPowerWaterInjection;
        }

        totalUseOfPowerWaterInjection.StartYear = drainageStrategy.ProductionProfileWaterInjection.StartYear;
        totalUseOfPowerWaterInjection.Values = waterInjectionValues;

        return totalUseOfPowerWaterInjection;
    }

    private TimeSeriesVolume CalculateTotalUseOfPowerGas(Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerGas = new TimeSeriesVolume();
        var co2ShareGasProfile = topside.CO2ShareGasProfile;
        var co2OnMaxGas = topside.CO2OnMaxGasProfile;
        var gasValues = Array.Empty<double>();

        if (drainageStrategy.ProductionProfileGas == null)
        {
            return totalUseOfPowerGas;
        }

        var gasRates = drainageStrategy.ProductionProfileGas.Values;
        for (var i = 0; i <= gasRates.Length; i++)
        {
            var calculatedPowerGas =
                co2ShareGasProfile * co2OnMaxGas + gasRates[i] * co2ShareGasProfile * (1 - co2OnMaxGas);

            gasValues[i] = calculatedPowerGas;
        }

        totalUseOfPowerGas.StartYear = drainageStrategy.ProductionProfileGas.StartYear;
        totalUseOfPowerGas.Values = gasValues;

        return totalUseOfPowerGas;
    }

    private TimeSeriesVolume CalculateTotalUseOfPowerOil(Topside topside, DrainageStrategy drainageStrategy)
    {
        var totalUseOfPowerOil = new TimeSeriesVolume();
        var co2ShareOilProfile = topside.CO2ShareOilProfile;
        var co2OnMaxOil = topside.CO2OnMaxOilProfile;
        var oilValues = Array.Empty<double>();

        if (drainageStrategy.ProductionProfileOil == null)
        {
            return totalUseOfPowerOil;
        }

        var oilRates = drainageStrategy.ProductionProfileOil.Values;
        for (var i = 0; i <= oilRates.Length; i++)
        {
            var calculatedPowerOil =
                co2ShareOilProfile * co2OnMaxOil + oilRates[i] * co2ShareOilProfile * (1 - co2OnMaxOil);
            oilValues[i] = calculatedPowerOil;
        }

        totalUseOfPowerOil.StartYear = drainageStrategy.ProductionProfileOil.StartYear;
        totalUseOfPowerOil.Values = oilValues;

        return totalUseOfPowerOil;
    }
}
