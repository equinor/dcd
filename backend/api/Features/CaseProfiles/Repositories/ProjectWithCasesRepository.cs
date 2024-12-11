using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Repositories;

public interface IProjectWithAssetsRepository
{
    Task<Project> GetProjectWithCases(Guid projectPk);
    Task<Project> GetProjectWithCasesAndAssets(Guid projectPk);
}

public class ProjectWithCasesRepository(DcdDbContext context) : IProjectWithAssetsRepository
{
    public async Task<Project> GetProjectWithCases(Guid projectPk)
    {
        return await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == projectPk);
    }

    public async Task<Project> GetProjectWithCasesAndAssets(Guid projectPk)
    {
        var project = await context.Projects
            .Include(p => p.Cases).ThenInclude(c => c.TotalFeasibilityAndConceptStudies)
            .Include(p => p.Cases).ThenInclude(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(p => p.Cases).ThenInclude(c => c.TotalFEEDStudies)
            .Include(p => p.Cases).ThenInclude(c => c.TotalFEEDStudiesOverride)
            .Include(p => p.Cases).ThenInclude(c => c.TotalOtherStudiesCostProfile)
            .Include(p => p.Cases).ThenInclude(c => c.WellInterventionCostProfile)
            .Include(p => p.Cases).ThenInclude(c => c.WellInterventionCostProfileOverride)
            .Include(p => p.Cases).ThenInclude(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(p => p.Cases).ThenInclude(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(p => p.Cases).ThenInclude(c => c.HistoricCostCostProfile)
            .Include(p => p.Cases).ThenInclude(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(p => p.Cases).ThenInclude(c => c.AdditionalOPEXCostProfile)
            .Include(p => p.Cases).ThenInclude(c => c.CessationWellsCost)
            .Include(p => p.Cases).ThenInclude(c => c.CessationWellsCostOverride)
            .Include(p => p.Cases).ThenInclude(c => c.CessationOffshoreFacilitiesCost)
            .Include(p => p.Cases).ThenInclude(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(p => p.Cases).ThenInclude(c => c.CessationOnshoreFacilitiesCostProfile)
            .Include(p => p.Cases).ThenInclude(c => c.CalculatedTotalIncomeCostProfile)
            .Include(p => p.Cases).ThenInclude(c => c.CalculatedTotalCostCostProfile)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == projectPk);

        project.Cases = project.Cases.OrderBy(c => c.CreateTime).ToList();

        await LoadProjectAssets(project);

        return project;
    }

    private async Task LoadProjectAssets(Project project)
    {
        project.WellProjects = await GetWellProjects(project.Id);
        project.DrainageStrategies = await GetDrainageStrategies(project.Id);
        project.Surfs = await GetSurfs(project.Id);
        project.Substructures = await GetSubstructures(project.Id);
        project.Topsides = await GetTopsides(project.Id);
        project.Transports = await GetTransports(project.Id);
        project.OnshorePowerSupplies = await GetOnshorePowerSupplies(project.Id);
        project.Explorations = await GetExplorations(project.Id);
        project.Wells = await GetWells(project.Id);
    }

    private async Task<List<Well>> GetWells(Guid projectId)
    {
        return await context.Wells
            .Where(d => d.ProjectId.Equals(projectId))
            .ToListAsync();
    }

    private async Task<List<Exploration>> GetExplorations(Guid projectId)
    {
        return await context.Explorations
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.GAndGAdminCostOverride)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .Include(c => c.ExplorationWells).ThenInclude(ew => ew.DrillingSchedule)
            .Where(d => d.Project.Id.Equals(projectId))
            .ToListAsync();
    }

    private async Task<List<Transport>> GetTransports(Guid projectId)
    {
        return await context.Transports
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(c => c.Project.Id.Equals(projectId))
            .ToListAsync();
    }

    private async Task<List<Topside>> GetTopsides(Guid projectId)
    {
        return await context.Topsides
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(c => c.Project.Id.Equals(projectId))
            .ToListAsync();
    }

    private async Task<List<Surf>> GetSurfs(Guid projectId)
    {
        return await context.Surfs
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(c => c.Project.Id.Equals(projectId))
            .ToListAsync();
    }

    private async Task<List<OnshorePowerSupply>> GetOnshorePowerSupplies(Guid projectId)
    {
        return await context.OnshorePowerSupplies
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Where(c => c.Project.Id.Equals(projectId))
            .ToListAsync();
    }

    private async Task<List<DrainageStrategy>> GetDrainageStrategies(Guid projectId)
    {
        return await context.DrainageStrategies
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
            .Where(d => d.Project.Id.Equals(projectId))
            .ToListAsync();
    }

    private async Task<List<WellProject>> GetWellProjects(Guid projectId)
    {
        return await context.WellProjects
            .Include(c => c.OilProducerCostProfile)
            .Include(c => c.OilProducerCostProfileOverride)
            .Include(c => c.GasProducerCostProfile)
            .Include(c => c.GasProducerCostProfileOverride)
            .Include(c => c.WaterInjectorCostProfile)
            .Include(c => c.WaterInjectorCostProfileOverride)
            .Include(c => c.GasInjectorCostProfile)
            .Include(c => c.GasInjectorCostProfileOverride)
            .Include(c => c.WellProjectWells).ThenInclude(wpw => wpw.DrillingSchedule)
            .Where(d => d.Project.Id.Equals(projectId))
            .ToListAsync();
    }

    private async Task<List<Substructure>> GetSubstructures(Guid projectId)
    {
        return await context.Substructures
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .Where(c => c.Project.Id.Equals(projectId))
            .ToListAsync();
    }
}
