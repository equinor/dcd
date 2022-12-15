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

        var totalExportedVolumes = GetTotalExportedVolumes(project,drainageStrategy);
        // var co2EmissionsProfile = GetCo2EmissionsProfile(project, drainageStrategy, caseId);
        var generateCo2EmissionsProfile = _generateCo2EmissionsProfile.Generate(caseId);

        // Co2emissions / totalExportedVolumes. iterate each value and divide
        var co2IntensityValues = new List<double>();
        
        if (generateCo2EmissionsProfile.StartYear == totalExportedVolumes.StartYear
            && generateCo2EmissionsProfile.Values.Length == totalExportedVolumes.Values.Length) 
        {
            for (var i = 0; i < generateCo2EmissionsProfile.Values.Length; i++)
            {
                co2IntensityValues[i] = generateCo2EmissionsProfile.Values[i] / totalExportedVolumes.Values[i];
            }
        }

        // var totalProfile =
        //     TimeSeriesCost.MergeCostProfiles(totalExportedVolumes, co2EmissionsProfile); // oil+gas. see comparecases
        var co2Intensity = new Co2Intensity
        {
            StartYear = generateCo2EmissionsProfile.StartYear,
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
        var oilValues = drainageStrategy.ProductionProfileOil?.Values.ToArray() ?? Array.Empty<double>();
        var oil = new TimeSeries<double>
        {
            Values = oilValues,
            StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
        };
        return oil;
    }

    private static TimeSeries<double> GetGasProfile(Project project, DrainageStrategy drainageStrategy)
    {
        var oilEquivalentFactor = 5.61;
        var gasValues = drainageStrategy.ProductionProfileGas?.Values.Select(v => v / oilEquivalentFactor ).ToArray() ?? Array.Empty<double>();
        var gas = new TimeSeries<double>
        {
            Values = gasValues,
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };
        return gas;
    }

    // private static TimeSeries<double> GetCo2EmissionsProfile(Project project, DrainageStrategy drainageStrategy, Guid caseId)
    // {
    //     // var generateCo2EmissionsProfile = _generateCo2EmissionsProfile.Generate(caseId);
    // }
}
