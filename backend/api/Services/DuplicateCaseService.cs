using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class DuplicateCaseService : IDuplicateCaseService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<DuplicateCaseService> _logger;

    public DuplicateCaseService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory
    )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<DuplicateCaseService>();
    }

    private async Task<Case> GetCaseNoTracking(Guid caseId)
    {
        var caseItem = await _context.Cases
            .AsNoTracking()
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudies)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .FirstOrDefaultAsync(c => c.Id == caseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
        }
        return caseItem;
    }

    public async Task<ProjectDto> DuplicateCase(Guid caseId)
    {
        var caseItem = await GetCaseNoTracking(caseId);

        var sourceWellProjectId = caseItem.WellProjectLink;
        var sourceExplorationId = caseItem.ExplorationLink;

        caseItem.CreateTime = DateTimeOffset.UtcNow;
        caseItem.ModifyTime = DateTimeOffset.UtcNow;
        caseItem.Id = new Guid();

        SetNewGuidTimeSeries(caseItem.TotalFeasibilityAndConceptStudies);
        SetNewGuidTimeSeries(caseItem.TotalFeasibilityAndConceptStudiesOverride);
        SetNewGuidTimeSeries(caseItem.TotalFEEDStudies);
        SetNewGuidTimeSeries(caseItem.TotalFEEDStudiesOverride);
        SetNewGuidTimeSeries(caseItem.TotalOtherStudies);
        SetNewGuidTimeSeries(caseItem.CessationWellsCost);
        SetNewGuidTimeSeries(caseItem.CessationWellsCostOverride);
        SetNewGuidTimeSeries(caseItem.CessationOffshoreFacilitiesCost);
        SetNewGuidTimeSeries(caseItem.CessationOffshoreFacilitiesCostOverride);
        SetNewGuidTimeSeries(caseItem.CessationOnshoreFacilitiesCostProfile);
        SetNewGuidTimeSeries(caseItem.WellInterventionCostProfile);
        SetNewGuidTimeSeries(caseItem.WellInterventionCostProfileOverride);
        SetNewGuidTimeSeries(caseItem.OffshoreFacilitiesOperationsCostProfile);
        SetNewGuidTimeSeries(caseItem.OffshoreFacilitiesOperationsCostProfileOverride);
        SetNewGuidTimeSeries(caseItem.HistoricCostCostProfile);
        SetNewGuidTimeSeries(caseItem.OnshoreRelatedOPEXCostProfile);
        SetNewGuidTimeSeries(caseItem.AdditionalOPEXCostProfile);

        var project = await _projectService.GetProject(caseItem.ProjectId);
        caseItem.Project = project;
        if (project.Cases != null)
        {
            caseItem.Name = GetUniqueCopyName(project.Cases, caseItem.Name);
        }

        var newDrainageStrategy = await CopyDrainageStrategy(caseItem.DrainageStrategyLink);
        var newTopside = await CopyTopside(caseItem.TopsideLink);
        var newSurf = await CopySurf(caseItem.SurfLink);
        var newSubstructure = await CopySubstructure(caseItem.SubstructureLink);
        var newTransport = await CopyTransport(caseItem.TransportLink);

        var newWellProject = await CopyWellProject(caseItem.WellProjectLink);
        var newExploration = await CopyExploration(caseItem.ExplorationLink);

        await CopyWellProjectWell(sourceWellProjectId, newWellProject.Id);
        await CopyExplorationWell(sourceExplorationId, newExploration.Id);

        caseItem.DrainageStrategyLink = newDrainageStrategy.Id;
        caseItem.TopsideLink = newTopside.Id;
        caseItem.SurfLink = newSurf.Id;
        caseItem.SubstructureLink = newSubstructure.Id;
        caseItem.TransportLink = newTransport.Id;

        caseItem.WellProjectLink = newWellProject.Id;
        caseItem.ExplorationLink = newExploration.Id;

        _context.Cases.Add(caseItem);

        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(project.Id);
    }

    private async Task<List<ExplorationWell>> CopyExplorationWell(Guid sourceExplorationId, Guid targetExplorationId)
    {
        var sourceExplorationWells = await GetAllExplorationWellForExplorationNoTracking(sourceExplorationId);

        foreach (var explorationWell in sourceExplorationWells)
        {
            explorationWell.ExplorationId = targetExplorationId;
            SetNewGuidTimeSeries(explorationWell.DrillingSchedule);

            _context.ExplorationWell.Add(explorationWell);
        }
        return sourceExplorationWells;
    }

    private async Task<List<ExplorationWell>> GetAllExplorationWellForExplorationNoTracking(Guid guid)
    {
        return await _context.ExplorationWell
                   .AsNoTracking()
                   .Where(ew => ew.ExplorationId == guid)
                   .Include(ew => ew.DrillingSchedule)
                   .ToListAsync();
    }

    private async Task<List<WellProjectWell>> CopyWellProjectWell(Guid sourceWellProjectId, Guid targetWellProjectId)
    {
        var sourceWellProjectWells = await GetAllWellProjectWellForWellProjectNoTracking(sourceWellProjectId);

        foreach (var wellProjectWell in sourceWellProjectWells)
        {
            wellProjectWell.WellProjectId = targetWellProjectId;
            SetNewGuidTimeSeries(wellProjectWell.DrillingSchedule);

            _context.WellProjectWell.Add(wellProjectWell);
        }
        return sourceWellProjectWells;
    }

    private async Task<List<WellProjectWell>> GetAllWellProjectWellForWellProjectNoTracking(Guid guid)
    {
        return await _context.WellProjectWell
            .AsNoTracking()
            .Where(wpw => wpw.WellProjectId == guid)
            .Include(wpw => wpw.DrillingSchedule)
            .ToListAsync();
    }



    private async Task<Topside> CopyTopside(Guid guid)
    {
        var newTopside = await GetTopsideNoTracking(guid);
        newTopside.Id = Guid.NewGuid();

        SetNewGuidTimeSeries(newTopside.CostProfile);
        SetNewGuidTimeSeries(newTopside.CostProfileOverride);
        SetNewGuidTimeSeries(newTopside.CessationCostProfile);

        _context.Topsides.Add(newTopside);
        return newTopside;
    }

    private async Task<Topside> GetTopsideNoTracking(Guid topsideId)
    {
        var topside = await _context.Topsides
            .AsNoTracking()
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == topsideId);
        if (topside == null)
        {
            throw new NotFoundInDBException(string.Format("Topside {0} not found.", topsideId));
        }
        return topside;
    }

    private async Task<Transport> CopyTransport(Guid guid)
    {
        var newTransport = await GetTransportNoTracking(guid);
        newTransport.Id = Guid.NewGuid();

        SetNewGuidTimeSeries(newTransport.CostProfile);
        SetNewGuidTimeSeries(newTransport.CostProfileOverride);
        SetNewGuidTimeSeries(newTransport.CessationCostProfile);

        _context.Transports.Add(newTransport);
        return newTransport;
    }

    private async Task<Transport> GetTransportNoTracking(Guid transportId)
    {
        var transport = await _context.Transports
            .AsNoTracking()
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(c => c.Id == transportId);
        if (transport == null)
        {
            throw new NotFoundInDBException(string.Format("Transport {0} not found.", transportId));
        }
        return transport;
    }

    private async Task<Substructure> CopySubstructure(Guid guid)
    {
        var newSubstructure = await GetSubstructureNoTracking(guid);
        newSubstructure.Id = Guid.NewGuid();

        SetNewGuidTimeSeries(newSubstructure.CostProfile);
        SetNewGuidTimeSeries(newSubstructure.CostProfileOverride);
        SetNewGuidTimeSeries(newSubstructure.CessationCostProfile);

        _context.Substructures.Add(newSubstructure);
        return newSubstructure;
    }

    private async Task<Substructure> GetSubstructureNoTracking(Guid substructureId)
    {
        var substructure = await _context.Substructures
            .AsNoTracking()
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == substructureId);
        if (substructure == null)
        {
            throw new NotFoundInDBException(string.Format("Substructure {0} not found.", substructureId));
        }
        return substructure;
    }

    private async Task<Surf> CopySurf(Guid guid)
    {
        var newSurf = await GetSurfNoTracking(guid);
        newSurf.Id = Guid.NewGuid();

        SetNewGuidTimeSeries(newSurf.CostProfile);
        SetNewGuidTimeSeries(newSurf.CostProfileOverride);
        SetNewGuidTimeSeries(newSurf.CessationCostProfile);

        _context.Surfs.Add(newSurf);
        return newSurf;
    }

    private async Task<Surf> GetSurfNoTracking(Guid surfId)
    {
        var surf = await _context.Surfs
            .AsNoTracking()
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == surfId);
        if (surf == null)
        {
            throw new NotFoundInDBException(string.Format("Surf {0} not found.", surfId));
        }
        return surf;
    }

    private async Task<DrainageStrategy> CopyDrainageStrategy(Guid guid)
    {
        var newDrainageStrategy = await GetDrainageStrategyNoTracking(guid);
        newDrainageStrategy.Id = Guid.NewGuid();

        SetNewGuidTimeSeries(newDrainageStrategy.ProductionProfileOil);
        SetNewGuidTimeSeries(newDrainageStrategy.ProductionProfileGas);
        SetNewGuidTimeSeries(newDrainageStrategy.ProductionProfileWater);
        SetNewGuidTimeSeries(newDrainageStrategy.ProductionProfileWaterInjection);
        SetNewGuidTimeSeries(newDrainageStrategy.FuelFlaringAndLosses);
        SetNewGuidTimeSeries(newDrainageStrategy.FuelFlaringAndLossesOverride);
        SetNewGuidTimeSeries(newDrainageStrategy.NetSalesGas);
        SetNewGuidTimeSeries(newDrainageStrategy.NetSalesGasOverride);
        SetNewGuidTimeSeries(newDrainageStrategy.Co2Emissions);
        SetNewGuidTimeSeries(newDrainageStrategy.Co2EmissionsOverride);
        SetNewGuidTimeSeries(newDrainageStrategy.ProductionProfileNGL);
        SetNewGuidTimeSeries(newDrainageStrategy.ImportedElectricity);
        SetNewGuidTimeSeries(newDrainageStrategy.ImportedElectricityOverride);
        SetNewGuidTimeSeries(newDrainageStrategy.DeferredOilProduction);
        SetNewGuidTimeSeries(newDrainageStrategy.DeferredGasProduction);

        _context.DrainageStrategies.Add(newDrainageStrategy);
        return newDrainageStrategy;
    }

    private async Task<DrainageStrategy> GetDrainageStrategyNoTracking(Guid drainageStrategyId)
    {
        var drainageStrategy = await _context.DrainageStrategies
            .AsNoTracking()
            .Include(c => c.ProductionProfileOil)
            .Include(c => c.ProductionProfileGas)
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
            .FirstOrDefaultAsync(o => o.Id == drainageStrategyId);
        if (drainageStrategy == null)
        {
            throw new NotFoundInDBException(string.Format("Drainage strategy {0} not found.", drainageStrategyId));
        }
        return drainageStrategy;
    }

    private async Task<Exploration> CopyExploration(Guid explorationId)
    {
        var newExploration = await GetExplorationNoTracking(explorationId);
        newExploration.Id = Guid.NewGuid();

        SetNewGuidTimeSeries(newExploration.ExplorationWellCostProfile);
        SetNewGuidTimeSeries(newExploration.AppraisalWellCostProfile);
        SetNewGuidTimeSeries(newExploration.SidetrackCostProfile);
        SetNewGuidTimeSeries(newExploration.GAndGAdminCost);
        SetNewGuidTimeSeries(newExploration.SeismicAcquisitionAndProcessing);
        SetNewGuidTimeSeries(newExploration.CountryOfficeCost);

        _context.Explorations.Add(newExploration);
        return newExploration;
    }

    private async Task<Exploration> GetExplorationNoTracking(Guid explorationId)
    {
        var exploration = await _context.Explorations
            .AsNoTracking()
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .FirstOrDefaultAsync(o => o.Id == explorationId);
        if (exploration == null)
        {
            throw new NotFoundInDBException(string.Format("Exploration {0} not found.", explorationId));
        }
        return exploration;
    }

    private async Task<WellProject> CopyWellProject(Guid guid)
    {
        var newWellProject = await GetWellProjectNoTracking(guid);
        newWellProject.Id = Guid.NewGuid();

        SetNewGuidTimeSeries(newWellProject.OilProducerCostProfile);
        SetNewGuidTimeSeries(newWellProject.OilProducerCostProfileOverride);
        SetNewGuidTimeSeries(newWellProject.GasProducerCostProfile);
        SetNewGuidTimeSeries(newWellProject.GasProducerCostProfileOverride);
        SetNewGuidTimeSeries(newWellProject.WaterInjectorCostProfile);
        SetNewGuidTimeSeries(newWellProject.WaterInjectorCostProfileOverride);
        SetNewGuidTimeSeries(newWellProject.GasInjectorCostProfile);
        SetNewGuidTimeSeries(newWellProject.GasInjectorCostProfileOverride);

        _context.WellProjects.Add(newWellProject);
        return newWellProject;
    }



    private async Task<WellProject> GetWellProjectNoTracking(Guid wellProjectId)
    {
        var wellProject = await _context.WellProjects
            .AsNoTracking()
            .Include(c => c.OilProducerCostProfile)
            .Include(c => c.OilProducerCostProfileOverride)
            .Include(c => c.GasProducerCostProfile)
            .Include(c => c.GasProducerCostProfileOverride)
            .Include(c => c.WaterInjectorCostProfile)
            .Include(c => c.WaterInjectorCostProfileOverride)
            .Include(c => c.GasInjectorCostProfile)
            .Include(c => c.GasInjectorCostProfileOverride)
            .FirstOrDefaultAsync(o => o.Id == wellProjectId);
        if (wellProject == null)
        {
            throw new NotFoundInDBException(string.Format("Well project {0} not found.", wellProjectId));
        }
        return wellProject;
    }

    private static void SetNewGuidTimeSeries(TimeSeries<double>? timeSeries)
    {
        if (timeSeries != null)
        {
            timeSeries.Id = Guid.NewGuid();
        }
    }

    private static void SetNewGuidTimeSeries(TimeSeries<int>? timeSeries)
    {
        if (timeSeries != null)
        {
            timeSeries.Id = Guid.NewGuid();
        }
    }

    private string GetUniqueCopyName(IEnumerable<Case> cases, string originalName)
    {
        var copyName = " - copy";
        var newName = originalName + copyName;
        var i = 1;

        string potentialName = newName;
        while (cases.Any(c => c.Name == potentialName))
        {
            i++;
            potentialName = newName + $" ({i})";
        }

        return potentialName;
    }
}
