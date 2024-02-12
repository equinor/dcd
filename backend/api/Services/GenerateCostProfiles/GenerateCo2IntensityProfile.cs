using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

using AutoMapper;

namespace api.Services.GenerateCostProfiles;

public class GenerateCo2IntensityProfile : IGenerateCo2IntensityProfile
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly IGenerateCo2EmissionsProfile _generateCo2EmissionsProfile;
    private readonly IMapper _mapper;

    public GenerateCo2IntensityProfile(
        ICaseService caseService, 
        IDrainageStrategyService drainageStrategyService, 
        IProjectService projectService,
        IGenerateCo2EmissionsProfile generateCo2EmissionsProfile,
        IMapper mapper
        )
    {
        _caseService = caseService;
        _projectService = projectService;
        _drainageStrategyService = drainageStrategyService;
        _generateCo2EmissionsProfile = generateCo2EmissionsProfile;
        _mapper = mapper;
    }

    public async Task<Co2IntensityDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);
        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var totalExportedVolumes = GetTotalExportedVolumes(drainageStrategy);

        TimeSeries<double> generateCo2EmissionsProfile = new();
        if (drainageStrategy.Co2EmissionsOverride?.Override == true)
        {
            generateCo2EmissionsProfile.StartYear = drainageStrategy.Co2EmissionsOverride.StartYear;
            generateCo2EmissionsProfile.Values = drainageStrategy.Co2EmissionsOverride.Values.Select(v => v / 1E6).ToArray();
        }
        else
        {
            var generatedCo2 = _generateCo2EmissionsProfile.GenerateAsync(caseId).GetAwaiter().GetResult();
            generateCo2EmissionsProfile.StartYear = generatedCo2.StartYear;
            generateCo2EmissionsProfile.Values = generatedCo2.Values;
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

        var dto = _mapper.Map<Co2IntensityDto>(co2Intensity);

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
        var oil = new TimeSeries<double>
        {
            Values = oilValues,
            StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
        };
        return oil;
    }

    private static TimeSeries<double> GetGasProfile(DrainageStrategy drainageStrategy)
    {
        var billion = 1E9;
        var gasValues = drainageStrategy.ProductionProfileGas?.Values.Select(v => v / billion).ToArray() ?? Array.Empty<double>();
        var gas = new TimeSeries<double>
        {
            Values = gasValues,
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };
        return gas;
    }
}
