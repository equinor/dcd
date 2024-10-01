using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class RevisionRepository : BaseRepository, IRevisionRepository
{
    private readonly ILogger<ProjectRepository> _logger;

    public RevisionRepository(
        DcdDbContext context,
        ILogger<ProjectRepository> logger
    ) : base(context)
    {
        _logger = logger;
    }

    public async Task<Project> AddRevision(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return project;
    }

    public async Task<Project?> GetProjectAndAssetsNoTracking(Guid id)
    {
        var projectQuery = _context.Projects.AsQueryable();
        projectQuery = IncludeProjectDetails(projectQuery);
        projectQuery = IncludeCaseDetails(projectQuery);
        projectQuery = IncludeDrainageStrategyDetails(projectQuery);
        projectQuery = IncludeExplorationDetails(projectQuery);
        projectQuery = IncludeWellProjectDetails(projectQuery);
        projectQuery = IncludeTransportDetails(projectQuery);
        projectQuery = IncludeTopsideDetails(projectQuery);
        projectQuery = IncludeSurfDetails(projectQuery);
        projectQuery = IncludeSubstructureDetails(projectQuery);

        projectQuery = projectQuery.AsNoTracking();

        var project = await projectQuery.FirstOrDefaultAsync(p => p.Id.Equals(id) && !p.IsRevision)
            ?? throw new NotFoundInDBException($"Project with id {id} not found.");

        return project;
    }

    private static IQueryable<Project> IncludeProjectDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts);
    }

    private static IQueryable<Project> IncludeCaseDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudies)
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudies)
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalFEEDStudiesOverride)
            .Include(p => p.Cases)!.ThenInclude(c => c.TotalOtherStudiesCostProfile)
            .Include(p => p.Cases)!.ThenInclude(c => c.WellInterventionCostProfile)
            .Include(p => p.Cases)!.ThenInclude(c => c.WellInterventionCostProfileOverride)
            .Include(p => p.Cases)!.ThenInclude(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(p => p.Cases)!.ThenInclude(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(p => p.Cases)!.ThenInclude(c => c.HistoricCostCostProfile)
            .Include(p => p.Cases)!.ThenInclude(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(p => p.Cases)!.ThenInclude(c => c.AdditionalOPEXCostProfile)
            .Include(p => p.Cases)!.ThenInclude(c => c.CessationWellsCost)
            .Include(p => p.Cases)!.ThenInclude(c => c.CessationWellsCostOverride)
            .Include(p => p.Cases)!.ThenInclude(c => c.CessationOffshoreFacilitiesCost)
            .Include(p => p.Cases)!.ThenInclude(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(p => p.Cases)!.ThenInclude(c => c.CessationOnshoreFacilitiesCostProfile);
    }

    private static IQueryable<Project> IncludeDrainageStrategyDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.ProductionProfileOil)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.AdditionalProductionProfileOil)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.ProductionProfileGas)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.AdditionalProductionProfileGas)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.ProductionProfileWater)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.ProductionProfileWaterInjection)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.FuelFlaringAndLosses)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.FuelFlaringAndLossesOverride)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.NetSalesGas)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.NetSalesGasOverride)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.Co2Emissions)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.Co2EmissionsOverride)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.ProductionProfileNGL)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.ImportedElectricity)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.ImportedElectricityOverride)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.DeferredOilProduction)
            .Include(p => p.DrainageStrategies)!.ThenInclude(c => c.DeferredGasProduction);
    }

    private static IQueryable<Project> IncludeExplorationDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.Explorations)!.ThenInclude(c => c.ExplorationWellCostProfile)
            .Include(p => p.Explorations)!.ThenInclude(c => c.AppraisalWellCostProfile)
            .Include(p => p.Explorations)!.ThenInclude(c => c.SidetrackCostProfile)
            .Include(p => p.Explorations)!.ThenInclude(c => c.GAndGAdminCost)
            .Include(p => p.Explorations)!.ThenInclude(c => c.GAndGAdminCostOverride)
            .Include(p => p.Explorations)!.ThenInclude(c => c.SeismicAcquisitionAndProcessing)
            .Include(p => p.Explorations)!.ThenInclude(c => c.CountryOfficeCost)
            .Include(p => p.Explorations)!.ThenInclude(c => c.ExplorationWells)!.ThenInclude(c => c.DrillingSchedule);
    }

    private static IQueryable<Project> IncludeWellProjectDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.WellProjects)!.ThenInclude(c => c.OilProducerCostProfile)
            .Include(p => p.WellProjects)!.ThenInclude(c => c.OilProducerCostProfileOverride)
            .Include(p => p.WellProjects)!.ThenInclude(c => c.GasProducerCostProfile)
            .Include(p => p.WellProjects)!.ThenInclude(c => c.GasProducerCostProfileOverride)
            .Include(p => p.WellProjects)!.ThenInclude(c => c.WaterInjectorCostProfile)
            .Include(p => p.WellProjects)!.ThenInclude(c => c.WaterInjectorCostProfileOverride)
            .Include(p => p.WellProjects)!.ThenInclude(c => c.GasInjectorCostProfile)
            .Include(p => p.WellProjects)!.ThenInclude(c => c.GasInjectorCostProfileOverride)
            .Include(p => p.WellProjects)!.ThenInclude(c => c.WellProjectWells)!.ThenInclude(c => c.DrillingSchedule);
    }

    private static IQueryable<Project> IncludeTransportDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.Transports)!.ThenInclude(c => c.CostProfile)
            .Include(p => p.Transports)!.ThenInclude(c => c.CostProfileOverride)
            .Include(p => p.Transports)!.ThenInclude(c => c.CessationCostProfile);
    }

    private static IQueryable<Project> IncludeTopsideDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.Topsides)!.ThenInclude(c => c.CostProfile)
            .Include(p => p.Topsides)!.ThenInclude(c => c.CostProfileOverride)
            .Include(p => p.Topsides)!.ThenInclude(c => c.CessationCostProfile);
    }

    private static IQueryable<Project> IncludeSurfDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.Surfs)!.ThenInclude(c => c.CostProfile)
            .Include(p => p.Surfs)!.ThenInclude(c => c.CostProfileOverride)
            .Include(p => p.Surfs)!.ThenInclude(c => c.CessationCostProfile);
    }

    private static IQueryable<Project> IncludeSubstructureDetails(IQueryable<Project> query)
    {
        return query
            .Include(p => p.Substructures)!.ThenInclude(c => c.CostProfile)
            .Include(p => p.Substructures)!.ThenInclude(c => c.CostProfileOverride)
            .Include(p => p.Substructures)!.ThenInclude(c => c.CessationCostProfile);
    }

}
