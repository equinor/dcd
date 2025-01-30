using System.Diagnostics;

using api.Context;
using api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;
using api.Features.Cases.Recalculation.Calculators.CalculateNpv;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;
using api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;
using api.Features.Cases.Recalculation.Types.CessationCostProfile;
using api.Features.Cases.Recalculation.Types.Co2EmissionsProfile;
using api.Features.Cases.Recalculation.Types.FuelFlaringLossesProfile;
using api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;
using api.Features.Cases.Recalculation.Types.ImportedElectricityProfile;
using api.Features.Cases.Recalculation.Types.NetSaleGasProfile;
using api.Features.Cases.Recalculation.Types.OpexCostProfile;
using api.Features.Cases.Recalculation.Types.StudyCostProfile;
using api.Features.Cases.Recalculation.Types.WellCostProfile;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class FullRecalculationService
{
    private readonly DcdDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public FullRecalculationService(DcdDbContext context, IServiceProvider serviceProvider)
    {
        context.ChangeTracker.LazyLoadingEnabled = false;
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task<Dictionary<string, object>> RunAllRecalculations(List<Guid> caseIds)
    {
        var stopwatch = Stopwatch.StartNew();
        var debugLogForProject = new Dictionary<string, object>();

        var caseData = await LoadCaseData(caseIds, debugLogForProject);

        foreach (var foo in caseData)
        {
            stopwatch.Restart();
            var caseItem = foo.CaseItem;
            var drillingSchedulesForExplorationWell = foo.DrillingSchedulesForExplorationWell;
            var drillingSchedulesForWellProjectWell = foo.DrillingSchedulesForWellProjectWell;

            var debugLogForCase = new Dictionary<string, long>();

            await _serviceProvider.GetRequiredService<WellCostProfileService>().UpdateCostProfilesForWells(caseItem.Id);
            debugLogForCase.Add("UpdateCostProfilesForWells", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            StudyCostProfileService.RunCalculation(caseItem);
            CessationCostProfileService.RunCalculation(caseItem, drillingSchedulesForWellProjectWell);
            FuelFlaringLossesProfileService.RunCalculation(caseItem);
            GenerateGAndGAdminCostProfile.RunCalculation(caseItem, drillingSchedulesForExplorationWell);
            ImportedElectricityProfileService.RunCalculation(caseItem);
            NetSaleGasProfileService.RunCalculation(caseItem);
            OpexCostProfileService.RunCalculation(caseItem, drillingSchedulesForWellProjectWell);
            Co2EmissionsProfileService.RunCalculation(caseItem, drillingSchedulesForWellProjectWell);
            Co2IntensityProfileService.RunCalculation(caseItem);
            CalculateTotalIncomeService.RunCalculation(caseItem);
            CalculateTotalCostService.RunCalculation(caseItem);
            CalculateNpvService.RunCalculation(caseItem);
            CalculateBreakEvenOilPriceService.RunCalculation(caseItem);

            debugLogForCase.Add("Run calculations", stopwatch.ElapsedMilliseconds);

            debugLogForProject.Add(caseItem.Id.ToString(), debugLogForCase);
        }

        stopwatch.Restart();

        await _context.SaveChangesAsync();

        debugLogForProject.Add("SaveChangesAsync", stopwatch.ElapsedMilliseconds);

        return debugLogForProject;
    }

    private async Task<List<CaseWithDrillingSchedules>> LoadCaseData(List<Guid> caseIds, Dictionary<string, object> debugLogForProject)
    {
        var stopwatch = Stopwatch.StartNew();

        var caseItems = await _context.Cases
            .Include(x => x.Project).ThenInclude(x => x.DevelopmentOperationalWellCosts)
            .Include(x => x.Surf)
            .Include(x => x.Topside)
            .Include(x => x.DrainageStrategy)
            .Where(x => caseIds.Contains(x.Id))
            .ToListAsync();

        debugLogForProject.Add("Case count", caseItems.Count);
        debugLogForProject.Add("Load cases", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await _context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.CaseId))
            .LoadAsync();

        debugLogForProject.Add("Time series profiles count", caseItems.SelectMany(x => x.TimeSeriesProfiles).Count());
        debugLogForProject.Add("Load time series profiles", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        var drillingSchedulesForWellProjectWell = await (
                from caseItem in _context.Cases
                join wp in _context.WellProjects on caseItem.WellProjectLink equals wp.Id
                join wpw in _context.WellProjectWell on wp.Id equals wpw.WellProjectId
                where wpw.DrillingSchedule != null
                select new
                {
                    CaseId = caseItem.Id,
                    DrillingSchedule = wpw.DrillingSchedule!
                })
            .GroupBy(x => x.CaseId, x => x.DrillingSchedule)
            .ToDictionaryAsync(x => x.Key, x => x.ToList());

        debugLogForProject.Add("drillingSchedulesForWellProjectWell count", drillingSchedulesForWellProjectWell.SelectMany(x => x.Value).Count());
        debugLogForProject.Add("Load drillingSchedulesForWellProjectWell", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        var drillingSchedulesForExplorationWell = await (
                from caseItem in _context.Cases
                join ew in _context.ExplorationWell on caseItem.ExplorationLink equals ew.ExplorationId
                where ew.DrillingSchedule != null
                select new
                {
                    CaseId = caseItem.Id,
                    DrillingSchedule = ew.DrillingSchedule!
                })
            .GroupBy(x => x.CaseId, x => x.DrillingSchedule)
            .ToDictionaryAsync(x => x.Key, x => x.ToList());

        debugLogForProject.Add("drillingSchedulesForExplorationWell count", drillingSchedulesForWellProjectWell.SelectMany(x => x.Value).Count());
        debugLogForProject.Add("Load drillingSchedulesForExplorationWell", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        var res = caseItems.Select(caseItem => new CaseWithDrillingSchedules
        {
            CaseItem = caseItem,
            DrillingSchedulesForExplorationWell = drillingSchedulesForExplorationWell.TryGetValue(caseItem.Id, out var expSchedules) ? expSchedules : [],
            DrillingSchedulesForWellProjectWell = drillingSchedulesForWellProjectWell.TryGetValue(caseItem.Id, out var wpwSchedules) ? wpwSchedules : [],
        }).ToList();

        debugLogForProject.Add("Transform loaded data", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        return res;
    }
}

public class CaseWithDrillingSchedules
{
    public required Case CaseItem { get; set; }
    public required List<DrillingSchedule> DrillingSchedulesForWellProjectWell { get; set; }
    public required List<DrillingSchedule> DrillingSchedulesForExplorationWell { get; set; }
}
