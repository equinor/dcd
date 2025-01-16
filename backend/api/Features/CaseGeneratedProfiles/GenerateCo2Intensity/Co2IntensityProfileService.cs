using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseGeneratedProfiles.GenerateCo2Intensity;

public class Co2IntensityProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudiesCostProfile)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .Include(c => c.CalculatedTotalIncomeCostProfile)
            .Include(c => c.CalculatedTotalCostCostProfile)
            .SingleAsync(c => c.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.Co2Emissions)
            .Include(d => d.Co2EmissionsOverride)
            .Include(d => d.Co2Intensity)
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        var totalExportedVolumes = GetTotalExportedVolumes(drainageStrategy);

        TimeSeries<double> generateCo2EmissionsProfile = new();
        if (drainageStrategy.Co2EmissionsOverride?.Override == true)
        {
            generateCo2EmissionsProfile.StartYear = drainageStrategy.Co2EmissionsOverride.StartYear;
            generateCo2EmissionsProfile.Values = drainageStrategy.Co2EmissionsOverride.Values.Select(v => v / 1E6).ToArray();
        }
        else
        {
            var co2Emissions = drainageStrategy.Co2Emissions;
            generateCo2EmissionsProfile.StartYear = co2Emissions?.StartYear ?? 0;
            generateCo2EmissionsProfile.Values = co2Emissions?.Values ?? [];
        }

        var co2IntensityValues = new List<double>();

        const int tonnesToKgFactor = 1000;
        const double boeConversionFactor = 6.29;
        var yearDifference = 0;
        if (generateCo2EmissionsProfile.StartYear != totalExportedVolumes.StartYear)
        {
            yearDifference = generateCo2EmissionsProfile.StartYear - totalExportedVolumes.StartYear;
        }
        for (var i = 0; i < generateCo2EmissionsProfile.Values.Length; i++)
        {
            if (yearDifference + i < 0) { continue; }

            if ((i + yearDifference < totalExportedVolumes.Values.Length) && totalExportedVolumes.Values[i + yearDifference] != 0)
            {
                var dividedProfiles = generateCo2EmissionsProfile.Values[i] / totalExportedVolumes.Values[i + yearDifference];
                co2IntensityValues.Add(dividedProfiles / 1E6 / boeConversionFactor * tonnesToKgFactor);
            }
        }

        var co2YearOffset = yearDifference < 0 ? yearDifference : 0;

        drainageStrategy.Co2Intensity = new()
        {
            StartYear = generateCo2EmissionsProfile.StartYear - co2YearOffset,
            Values = co2IntensityValues.ToArray(),
        };
    }

    private static TimeSeries<double> GetTotalExportedVolumes(DrainageStrategy drainageStrategy)
    {
        var oilProfile = GetOilProfile(drainageStrategy);
        var gasProfile = GetGasProfile(drainageStrategy);

        var totalProfile = TimeSeriesCost.MergeCostProfiles(oilProfile, gasProfile);

        return new Co2Intensity
        {
            StartYear = totalProfile.StartYear,
            Values = totalProfile.Values
        };
    }

    private static TimeSeries<double> GetOilProfile(DrainageStrategy drainageStrategy)
    {
        var million = 1E6;
        var oilValues = drainageStrategy.ProductionProfileOil?.Values.Select(v => v / million).ToArray() ?? [];
        var additionalOilValues = drainageStrategy.AdditionalProductionProfileOil?.Values.Select(v => v / million).ToArray() ?? [];

        TimeSeriesCost? oilProfile = null;
        TimeSeriesCost? additionalOilProfile = null;

        if (drainageStrategy.ProductionProfileOil != null)
        {
            oilProfile = new TimeSeriesCost
            {
                StartYear = drainageStrategy.ProductionProfileOil.StartYear,
                Values = oilValues,
            };
        }

        if (drainageStrategy.AdditionalProductionProfileOil != null)
        {
            additionalOilProfile = new TimeSeriesCost
            {
                StartYear = drainageStrategy.AdditionalProductionProfileOil.StartYear,
                Values = additionalOilValues,
            };
        }

        // Merging the profiles, defaulting to an empty profile if null
        var mergedProfiles = TimeSeriesCost.MergeCostProfiles(
            oilProfile ?? new TimeSeriesCost { Values = [], StartYear = 0 },
            additionalOilProfile ?? new TimeSeriesCost { Values = [], StartYear = 0 }
        );

        return new TimeSeries<double>
        {
            Values = mergedProfiles.Values,
            StartYear = mergedProfiles.StartYear,
        };
    }

    private static TimeSeries<double> GetGasProfile(DrainageStrategy drainageStrategy)
    {
        var billion = 1E9;
        var gasValues = drainageStrategy.ProductionProfileGas?.Values.Select(v => v / billion).ToArray() ?? [];
        var additionalGasValues = drainageStrategy.AdditionalProductionProfileGas?.Values.Select(v => v / billion).ToArray() ?? [];

        TimeSeriesCost? gasProfile = null;
        TimeSeriesCost? additionalGasProfile = null;

        if (drainageStrategy.ProductionProfileGas != null)
        {
            gasProfile = new TimeSeriesCost
            {
                StartYear = drainageStrategy.ProductionProfileGas.StartYear,
                Values = gasValues,
            };
        }

        if (drainageStrategy.AdditionalProductionProfileGas != null)
        {
            additionalGasProfile = new TimeSeriesCost
            {
                StartYear = drainageStrategy.AdditionalProductionProfileGas.StartYear,
                Values = additionalGasValues,
            };
        }

        var mergedProfiles = TimeSeriesCost.MergeCostProfiles(
            gasProfile ?? new TimeSeriesCost { Values = [], StartYear = 0 },
            additionalGasProfile ?? new TimeSeriesCost { Values = [], StartYear = 0 }
        );

        return new TimeSeries<double>
        {
            Values = mergedProfiles.Values,
            StartYear = mergedProfiles.StartYear,
        };
    }
}
