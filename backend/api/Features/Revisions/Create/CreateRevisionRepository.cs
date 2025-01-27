using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Create;

public class CreateRevisionRepository(DcdDbContext context)
{
    public async Task<Project> GetDetachedProjectGraph(Guid projectPk)
    {
        var project = await context.Projects
                          .Include(p => p.ExplorationOperationalWellCosts)
                          .Include(p => p.DevelopmentOperationalWellCosts)
                          .SingleAsync(p => p.Id == projectPk);

        await LoadCases(projectPk);
        await LoadDrainageStrategies(projectPk);
        await LoadExplorations(projectPk);
        await LoadWellProjects(projectPk);
        await LoadTransports(projectPk);
        await LoadTopsides(projectPk);
        await LoadSurfs(projectPk);
        await LoadSubstructures(projectPk);
        await LoadOnshorePowerSupplies(projectPk);

        DetachEntriesToEnablePrimaryKeyEdits();

        return project;
    }

    private async Task LoadCases(Guid projectPk)
    {
        await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .Include(c => c.WellProject).ThenInclude(wp => wp!.WellProjectWells).ThenInclude(c => c.Well)
            .Include(c => c.Exploration).ThenInclude(wp => wp!.ExplorationWells).ThenInclude(c => c.Well)
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();
    }

    private async Task LoadDrainageStrategies(Guid projectPk)
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
            .Include(c => c.Co2Intensity)
            .Include(c => c.ProductionProfileNgl)
            .Include(c => c.ImportedElectricity)
            .Include(c => c.ImportedElectricityOverride)
            .Include(c => c.DeferredOilProduction)
            .Include(c => c.DeferredGasProduction)
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();
    }

    private async Task LoadExplorations(Guid projectPk)
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
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();
    }

    private async Task LoadWellProjects(Guid projectPk)
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
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();
    }

    private async Task LoadTransports(Guid projectPk)
    {
        await context.Transports
            .Include(c => c.CessationCostProfile)
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();
    }

    private async Task LoadTopsides(Guid projectPk)
    {
        await context.Topsides
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();
    }

    private async Task LoadSurfs(Guid projectPk)
    {
        await context.Surfs
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();
    }

    private async Task LoadSubstructures(Guid projectPk)
    {
        await context.Substructures
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(x => x.ProjectId == projectPk)
            .LoadAsync();
    }

    private async Task LoadOnshorePowerSupplies(Guid projectPk)
    {
        await context.OnshorePowerSupplies
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Where(x => x.ProjectId == projectPk)
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
