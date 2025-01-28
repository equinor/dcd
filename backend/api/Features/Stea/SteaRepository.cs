using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Stea;

public class SteaRepository(DcdDbContext context)
{
    public async Task<Project> GetProjectWithCasesAndAssets(Guid projectPk)
    {
        var project = await context.Projects
            .Include(p => p.Cases).ThenInclude(c => c.TimeSeriesProfiles)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == projectPk);

        project.Cases = project.Cases.OrderBy(c => c.CreatedUtc).ToList();

        return project;
    }

    public async Task<List<Exploration>> GetExplorations(Guid projectPk)
    {
        return await context.Explorations
            .Include(c => c.ExplorationWells).ThenInclude(ew => ew.DrillingSchedule)
            .Where(d => d.ProjectId == projectPk)
            .ToListAsync();
    }

    public async Task<List<Transport>> GetTransports(Guid projectPk)
    {
        return await context.Transports
            .Where(c => c.ProjectId == projectPk)
            .ToListAsync();
    }

    public async Task<List<Topside>> GetTopsides(Guid projectPk)
    {
        return await context.Topsides
            .Where(c => c.ProjectId == projectPk)
            .ToListAsync();
    }

    public async Task<List<Surf>> GetSurfs(Guid projectPk)
    {
        return await context.Surfs
            .Where(c => c.ProjectId == projectPk)
            .ToListAsync();
    }

    public async Task<List<OnshorePowerSupply>> GetOnshorePowerSupplies(Guid projectPk)
    {
        return await context.OnshorePowerSupplies
            .Where(c => c.ProjectId == projectPk)
            .ToListAsync();
    }

    public async Task<List<DrainageStrategy>> GetDrainageStrategies(Guid projectPk)
    {
        return await context.DrainageStrategies
            .Include(c => c.ProductionProfileOil)
            .Include(c => c.AdditionalProductionProfileOil)
            .Include(c => c.ProductionProfileGas)
            .Include(c => c.AdditionalProductionProfileGas)
            .Include(c => c.ProductionProfileWater)
            .Include(c => c.ProductionProfileWaterInjection)
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
            .Where(d => d.ProjectId == projectPk)
            .ToListAsync();
    }

    public async Task<List<WellProject>> GetWellProjects(Guid projectPk)
    {
        return await context.WellProjects
            .Include(c => c.WellProjectWells).ThenInclude(wpw => wpw.DrillingSchedule)
            .Where(d => d.ProjectId == projectPk)
            .ToListAsync();
    }

    public async Task<List<Substructure>> GetSubstructures(Guid projectPk)
    {
        return await context.Substructures
            .Where(c => c.ProjectId == projectPk)
            .ToListAsync();
    }
}
