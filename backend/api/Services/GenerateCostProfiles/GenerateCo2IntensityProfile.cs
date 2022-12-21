using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateCo2IntensityProfile
{
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ProjectService _projectService;
    private readonly GenerateCo2EmissionsProfile _generateCo2EmissionsProfile;

    public GenerateCo2IntensityProfile(IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _generateCo2EmissionsProfile = serviceProvider.GetRequiredService<GenerateCo2EmissionsProfile>();
    }

    public Co2IntensityDto Generate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var totalExportedVolumes = GetTotalExportedVolumes(project, drainageStrategy);
        var generateCo2EmissionsProfile = _generateCo2EmissionsProfile.Generate(caseId);

        var co2IntensityValues = new List<double>();

        var tonnesToKgFactor = 1000;
        var boeConversionFactor = 6.29;
        for (var i = 0; i < totalExportedVolumes.Values.Length; i++)
        {
            var yearDifference = 0;
            if (generateCo2EmissionsProfile.StartYear != totalExportedVolumes.StartYear)
            {
                yearDifference = totalExportedVolumes.StartYear - generateCo2EmissionsProfile.StartYear;
            }
            if ((i + yearDifference < generateCo2EmissionsProfile.Values.Length) && totalExportedVolumes.Values[i] != 0)
            {
                var dividedProfiles = generateCo2EmissionsProfile.Values[i + yearDifference] / totalExportedVolumes.Values[i];
                co2IntensityValues.Add(dividedProfiles / boeConversionFactor * tonnesToKgFactor);
            }
        }

        var co2Intensity = new Co2Intensity
        {
            StartYear = totalExportedVolumes.StartYear,
            Values = co2IntensityValues.ToArray(),
        };

        var dto = DrainageStrategyDtoAdapter.Convert(co2Intensity, project.PhysicalUnit);
        return dto ?? new Co2IntensityDto();
    }

    private static TimeSeries<double> GetTotalExportedVolumes(Project project, DrainageStrategy drainageStrategy)
    {
        var oilProfile = GetOilProfile(project, drainageStrategy);
        var gasProfile = GetGasProfile(project, drainageStrategy);

        var totalProfile =
            TimeSeriesCost.MergeCostProfiles(oilProfile, gasProfile);
        var totalExportedVolumes = new Co2Intensity
        {
            StartYear = totalProfile.StartYear,
            Values = totalProfile.Values,
        };
        return totalExportedVolumes;
    }

    private static TimeSeries<double> GetOilProfile(Project project, DrainageStrategy drainageStrategy)
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

    private static TimeSeries<double> GetGasProfile(Project project, DrainageStrategy drainageStrategy)
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
