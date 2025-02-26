using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Co2DrillingFlaringFuelTotals;

public class Co2DrillingFlaringFuelTotalsService(DcdDbContext context)
{
    public async Task<Co2DrillingFlaringFuelTotalsDto> Generate(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var profileTypes = new List<string>
        {
            ProfileTypes.ProductionProfileOil,
            ProfileTypes.AdditionalProductionProfileOil,
            ProfileTypes.ProductionProfileGas,
            ProfileTypes.AdditionalProductionProfileGas,
            ProfileTypes.ProductionProfileWaterInjection
        };

        var caseItem = await context.Cases
            .Include(x => x.Project)
            .Include(x => x.Topside)
            .SingleOrDefaultAsync(x => x.Project.Id == projectPk && x.Id == caseId);

        if (caseItem == null)
        {
            throw new NotFoundInDbException($"Case with id {caseId} and projectId {projectPk} not found.");
        }

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(y => profileTypes.Contains(y.ProfileType))
            .LoadAsync();

        var developmentWells = await context.Campaigns
            .Where(x => x.CaseId == caseId)
            .Where(x => x.CampaignType == CampaignType.DevelopmentCampaign)
            .SelectMany(x => x.CampaignWells)
            .ToListAsync();

        var fuelConsumptionsTotal = GetFuelConsumptionsProfileTotal(caseItem);
        var flaringsTotal = GetFlaringsProfileTotal(caseItem);
        var drillingEmissionsTotal = CalculateDrillingEmissionsTotal(caseItem.Project, developmentWells);

        return new Co2DrillingFlaringFuelTotalsDto
        {
            Co2Drilling = drillingEmissionsTotal,
            Co2Flaring = flaringsTotal,
            Co2Fuel = fuelConsumptionsTotal
        };
    }

    private static double GetFlaringsProfileTotal(Case caseItem)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(caseItem);

        var flaringsProfile = new TimeSeries
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * caseItem.Project.CO2EmissionsFromFlaredGas).ToArray()
        };

        return flaringsProfile.Values.Sum() / 1000;
    }

    private static double GetFuelConsumptionsProfileTotal(Case caseItem)
    {
        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem);

        var fuelConsumptionsProfile = new TimeSeries
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * caseItem.Project.CO2EmissionFromFuelGas).ToArray()
        };

        return fuelConsumptionsProfile.Values.Sum() / 1000;
    }

    private static double CalculateDrillingEmissionsTotal(Project project, List<CampaignWell> developmentWells)
    {
        var wellDrillingSchedules = new TimeSeries();

        foreach (var developmentWell in developmentWells)
        {
            var timeSeries = new TimeSeries
            {
                StartYear = developmentWell.StartYear,
                Values = developmentWell.Values.Select(v => (double)v).ToArray()
            };

            wellDrillingSchedules = TimeSeriesMerger.MergeTimeSeries(wellDrillingSchedules, timeSeries);
        }

        return wellDrillingSchedules.Values
            .Select(well => well * project.AverageDevelopmentDrillingDays * project.DailyEmissionFromDrillingRig)
            .Sum();
    }
}
