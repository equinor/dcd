using api.Context;
using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.Assets.CaseAssets.Surfs.Services;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public class CessationCostProfileService(
    DcdDbContext context,
    ICaseService caseService,
    IDrainageStrategyService drainageStrategyService,
    ISurfService surfService,
    IProjectWithAssetsRepository projectWithAssetsRepository)
    : ICessationCostProfileService
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(
            caseId,
            c => c.CessationWellsCostOverride!,
            c => c.CessationWellsCost!,
            c => c.CessationOffshoreFacilitiesCostOverride!,
            c => c.CessationOffshoreFacilitiesCost!
        );

        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!
        );

        var lastYearOfProduction = CalculationHelper.GetRelativeLastYearOfProduction(drainageStrategy);

        var project = await projectWithAssetsRepository.GetProjectWithCases(caseItem.ProjectId);

        await CalculateCessationWellsCost(caseItem, project, lastYearOfProduction);
        await GetCessationOffshoreFacilitiesCost(caseItem, lastYearOfProduction);
    }

    private async Task CalculateCessationWellsCost(Case caseItem, Project project, int? lastYear)
    {
        if (caseItem.CessationWellsCostOverride?.Override == true)
        {
            return;
        }

        if (!lastYear.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.CessationWellsCost);
            return;
        }

        caseItem.CessationWellsCost = await GenerateCessationWellsCost(
            caseItem.WellProjectLink,
            project,
            lastYear.Value,
            caseItem.CessationWellsCost ?? new CessationWellsCost()
        );
    }

    private async Task GetCessationOffshoreFacilitiesCost(Case caseItem, int? lastYear)
    {
        if (caseItem.CessationOffshoreFacilitiesCostOverride?.Override == true)
        {
            return;
        }

        if (!lastYear.HasValue)
        {
            CalculationHelper.ResetTimeSeries(caseItem.CessationOffshoreFacilitiesCost);
            return;
        }

        var surf = await surfService.GetSurfWithIncludes(caseItem.SurfLink);
        caseItem.CessationOffshoreFacilitiesCost = GenerateCessationOffshoreFacilitiesCost(
            surf,
            lastYear.Value,
            caseItem.CessationOffshoreFacilitiesCost ?? new CessationOffshoreFacilitiesCost()
        );
    }

    private async Task<CessationWellsCost> GenerateCessationWellsCost(Guid wellProjectId, Project project, int lastYear, CessationWellsCost cessationWells)
    {
        var linkedWells = await context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == wellProjectId).ToListAsync();

        var pluggingAndAbandonment = project.DevelopmentOperationalWellCosts?.PluggingAndAbandonment ?? 0;

        var sumDrilledWells = linkedWells
            .Select(well => well.DrillingSchedule?.Values.Sum() ?? 0)
            .Sum();

        var totalCost = sumDrilledWells * (double)pluggingAndAbandonment;
        cessationWells.StartYear = lastYear;
        cessationWells.Values = [totalCost / 2, totalCost / 2];

        return cessationWells;
    }

    private static CessationOffshoreFacilitiesCost GenerateCessationOffshoreFacilitiesCost(Surf surf, int lastYear, CessationOffshoreFacilitiesCost cessationOffshoreFacilities)
    {
        var surfCessationCost = surf.CessationCost;

        cessationOffshoreFacilities.StartYear = lastYear + 1;
        cessationOffshoreFacilities.Values = [(double)surfCessationCost / 2, (double)surfCessationCost / 2];
        return cessationOffshoreFacilities;
    }
}
