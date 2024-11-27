using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Duplicate;

public class DuplicateCaseRepository(DcdDbContext context)
{
    public async Task<Case> GetCaseAndAssetsNoTracking(Guid caseId)
    {
        var caseItem = await LoadCase(caseId);

        await LoadDrainageStrategies(caseItem.DrainageStrategyLink);
        await LoadExplorations(caseItem.ExplorationLink);
        await LoadWellProjects(caseItem.WellProjectLink);
        await LoadTransports(caseItem.TransportLink);
        await LoadTopsides(caseItem.TopsideLink);
        await LoadSurfs(caseItem.SurfLink);
        await LoadSubstructures(caseItem.SubstructureLink);

        DetachEntriesToEnablePrimaryKeyEdits();

        return caseItem;
    }

    private async Task<Case> LoadCase(Guid caseId)
    {
        return await context.Cases
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudiesCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .Include(c => c.CalculatedTotalCostCostProfile)
            .Include(c => c.CalculatedTotalIncomeCostProfile)
            .Include(c => c.CalculatedTotalCostCostProfile)
            .Where(x => x.Id == caseId)
            .FirstOrDefaultAsync() ?? throw new NotFoundInDBException($"Case {caseId} not found.");
    }

    private async Task LoadDrainageStrategies(Guid drainageStrategyLink)
    {
        await context.DrainageStrategies
            .Include(c => c.ProductionProfileOil)
            .Include(c => c.AdditionalProductionProfileOil)
            .Include(c => c.ProductionProfileGas)
            .Include(c => c.AdditionalProductionProfileGas)
            .Include(c => c.ProductionProfileWater)
            .Include(c => c.ProductionProfileWaterInjection)
            .Include(c => c.FuelFlaringAndLosses)
            .Include(c => c.FuelFlaringAndLossesOverride)
            .Include(c => c.NetSalesGas)
            .Include(c => c.NetSalesGasOverride)
            .Include(c => c.Co2Emissions)
            .Include(c => c.Co2EmissionsOverride)
            .Include(c => c.ProductionProfileNGL)
            .Include(c => c.ImportedElectricity)
            .Include(c => c.ImportedElectricityOverride)
            .Include(c => c.DeferredOilProduction)
            .Include(c => c.DeferredGasProduction)
            .Where(x => x.Id == drainageStrategyLink)
            .LoadAsync();
    }

    private async Task LoadExplorations(Guid explorationLink)
    {
        await context.Explorations
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.GAndGAdminCostOverride)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .Include(c => c.ExplorationWells).ThenInclude(c => c.DrillingSchedule)
            .Include(c => c.ExplorationWells).ThenInclude(c => c.Well)
            .Where(x => x.Id == explorationLink)
            .LoadAsync();
    }

    private async Task LoadWellProjects(Guid wellProjectLink)
    {
        await context.WellProjects
            .Include(c => c.OilProducerCostProfile)
            .Include(c => c.OilProducerCostProfileOverride)
            .Include(c => c.GasProducerCostProfile)
            .Include(c => c.GasProducerCostProfileOverride)
            .Include(c => c.WaterInjectorCostProfile)
            .Include(c => c.WaterInjectorCostProfileOverride)
            .Include(c => c.GasInjectorCostProfile)
            .Include(c => c.GasInjectorCostProfileOverride)
            .Include(c => c.WellProjectWells).ThenInclude(c => c.Well)
            .Include(c => c.WellProjectWells).ThenInclude(c => c.DrillingSchedule)
            .Where(x => x.Id == wellProjectLink)
            .LoadAsync();
    }

    private async Task LoadTransports(Guid transportLink)
    {
        await context.Transports
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.Id == transportLink)
            .LoadAsync();
    }

    private async Task LoadTopsides(Guid topsideLink)
    {
        await context.Topsides
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.Id == topsideLink)
            .LoadAsync();
    }

    private async Task LoadSurfs(Guid surfLink)
    {
        await context.Surfs
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.Id == surfLink)
            .LoadAsync();
    }

    private async Task LoadSubstructures(Guid substructureLink)
    {
        await context.Substructures
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.Id == substructureLink)
            .LoadAsync();
    }

    private void DetachEntriesToEnablePrimaryKeyEdits()
    {
        var entries = context.ChangeTracker
            .Entries()
            .Where(e => e.State != EntityState.Detached)
            .ToList();

        foreach (var entry in entries)
        {
            entry.State = EntityState.Detached;
        }
    }
}
