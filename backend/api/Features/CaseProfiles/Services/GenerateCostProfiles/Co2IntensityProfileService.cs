using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using AutoMapper;

namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public class Co2IntensityProfileService(
    ICaseService caseService,
    IDrainageStrategyService drainageStrategyService,
    IProjectWithAssetsRepository projectWithAssetsRepository,
    IMapper mapper)
    : ICo2IntensityProfileService
{
    public async Task<Co2IntensityDto> Generate(Guid caseId)
    {
        var caseItem = await caseService.GetCase(caseId);
        var project = await projectWithAssetsRepository.GetProjectWithCases(caseItem.ProjectId);
        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.Co2Emissions!,
            d => d.Co2EmissionsOverride!,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!
        );

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
                co2IntensityValues.Add(dividedProfiles / boeConversionFactor * tonnesToKgFactor);
            }
        }

        var co2YearOffset = yearDifference < 0 ? yearDifference : 0;

        var co2Intensity = new Co2Intensity
        {
            StartYear = generateCo2EmissionsProfile.StartYear - co2YearOffset,
            Values = co2IntensityValues.ToArray(),
        };

        var dto = mapper.Map<Co2IntensityDto>(co2Intensity);

        return dto ?? new Co2IntensityDto();
    }

    private static TimeSeries<double> GetTotalExportedVolumes(DrainageStrategy drainageStrategy)
    {
        var oilProfile = GetOilProfile(drainageStrategy);
        var gasProfile = GetGasProfile(drainageStrategy);

        var totalProfile =
            TimeSeriesCost.MergeCostProfiles(oilProfile, gasProfile);
        var totalExportedVolumes = new Co2Intensity
        {
            StartYear = totalProfile.StartYear,
            Values = totalProfile.Values,
        };
        return totalExportedVolumes;
    }

    private static TimeSeries<double> GetOilProfile(DrainageStrategy drainageStrategy)
    {
        var million = 1E6;
        var oilValues = drainageStrategy.ProductionProfileOil?.Values.Select(v => v / million).ToArray() ?? Array.Empty<double>();
        var additionalOilValues = drainageStrategy.AdditionalProductionProfileOil?.Values.Select(v => v / million).ToArray() ?? Array.Empty<double>();

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
            oilProfile ?? new TimeSeriesCost { Values = Array.Empty<double>(), StartYear = 0 },
            additionalOilProfile ?? new TimeSeriesCost { Values = Array.Empty<double>(), StartYear = 0 }
        );

        var oil = new TimeSeries<double>
        {
            Values = mergedProfiles.Values,
            StartYear = mergedProfiles.StartYear,
        };

        return oil;
    }


    private static TimeSeries<double> GetGasProfile(DrainageStrategy drainageStrategy)
    {
        var billion = 1E9;
        var gasValues = drainageStrategy.ProductionProfileGas?.Values.Select(v => v / billion).ToArray() ?? Array.Empty<double>();
        var additionalGasValues = drainageStrategy.AdditionalProductionProfileGas?.Values.Select(v => v / billion).ToArray() ?? Array.Empty<double>();

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
            gasProfile ?? new TimeSeriesCost { Values = Array.Empty<double>(), StartYear = 0 },
            additionalGasProfile ?? new TimeSeriesCost { Values = Array.Empty<double>(), StartYear = 0 }
        );

        var gas = new TimeSeries<double>
        {
            Values = mergedProfiles.Values,
            StartYear = mergedProfiles.StartYear,
        };

        return gas;
    }

}
