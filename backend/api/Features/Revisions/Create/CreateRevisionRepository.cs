using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Create;

public class CreateRevisionRepository(DcdDbContext context)
{
    public async Task<Project> GetProjectAndAssetsNoTracking(Guid projectId)
    {
        var project = await context.Projects
                          .Include(p => p.ExplorationOperationalWellCosts)
                          .Include(p => p.DevelopmentOperationalWellCosts)
                          .FirstOrDefaultAsync(p => p.Id == projectId && !p.IsRevision)
                      ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        await LoadCases(projectId);
        await LoadDrainageStrategies(projectId);
        await LoadExplorations(projectId);
        await LoadWellProjects(projectId);
        await LoadTransports(projectId);
        await LoadTopsides(projectId);
        await LoadSurfs(projectId);
        await LoadSubstructures(projectId);

        DetachEntriesToEnablePrimaryKeyEdits();

        project.ProjectMembers = [];

        return project;
    }

    private async Task LoadCases(Guid projectId)
    {
        await context.Cases
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
            .Include(c => c.WellProject).ThenInclude(wp => wp!.WellProjectWells).ThenInclude(c => c.Well)
            .Include(c => c.Exploration).ThenInclude(wp => wp!.ExplorationWells).ThenInclude(c => c.Well)
            .Where(x => x.ProjectId == projectId)
            .LoadAsync();
    }

    private async Task LoadDrainageStrategies(Guid projectId)
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
            .Where(x => x.ProjectId == projectId)
            .LoadAsync();
    }

    private async Task LoadExplorations(Guid projectId)
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
            .Where(x => x.ProjectId == projectId)
            .LoadAsync();
    }

    private async Task LoadWellProjects(Guid projectId)
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
            .Where(x => x.ProjectId == projectId)
            .LoadAsync();
    }

    private async Task LoadTransports(Guid projectId)
    {
        await context.Transports
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.ProjectId == projectId)
            .LoadAsync();
    }

    private async Task LoadTopsides(Guid projectId)
    {
        await context.Topsides
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.ProjectId == projectId)
            .LoadAsync();
    }

    private async Task LoadSurfs(Guid projectId)
    {
        await context.Surfs
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.ProjectId == projectId)
            .LoadAsync();
    }

    private async Task LoadSubstructures(Guid projectId)
    {
        await context.Substructures
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.ProjectId == projectId)
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
