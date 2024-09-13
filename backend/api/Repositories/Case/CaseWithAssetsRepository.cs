using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class CaseWithAssetsRepository : ICaseWithAssetsRepository
{
    private readonly DcdDbContext _context;
    private readonly ILogger<CaseRepository> _logger;

    public CaseWithAssetsRepository(
        DcdDbContext context,
        ILogger<CaseRepository> logger
        )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(
        Case CaseItem,
        DrainageStrategy DrainageStrategy,
        Topside Topside,
        Exploration Exploration,
        Substructure Substructure,
        Surf Surf,
        Transport Transport,
        WellProject WellProject
        )> GetCaseWithAssetsNoTracking(Guid caseId)
    {
        var caseItem = await GetCaseNoTracking(caseId);
        var drainageStrategy = await GetDrainageStrategyNoTracking(caseItem.DrainageStrategyLink);
        var topside = await GetTopsideNoTracking(caseItem.TopsideLink);
        var exploration = await GetExplorationNoTracking(caseItem.ExplorationLink);
        var substructure = await GetSubstructureNoTracking(caseItem.SubstructureLink);
        var surf = await GetSurfNoTracking(caseItem.SurfLink);
        var transport = await GetTransportNoTracking(caseItem.TransportLink);
        var wellProject = await GetWellProjectNoTracking(caseItem.WellProjectLink);

        return (caseItem, drainageStrategy, topside, exploration, substructure, surf, transport, wellProject);
    }

    private async Task<Case> GetCaseNoTracking(Guid id)
    {
        return await _context.Cases
                .Include(c => c.TotalFeasibilityAndConceptStudies)
                .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
                .Include(c => c.TotalFEEDStudies)
                .Include(c => c.TotalFEEDStudiesOverride)
                .Include(c => c.TotalOtherStudiesCostProfile)
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
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundInDBException($"Case with id {id} not found.");
    }

    private async Task<Transport> GetTransportNoTracking(Guid id)
    {
        return await _context.Transports
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundInDBException($"Transport with id {id} not found.");
    }

    private async Task<Surf> GetSurfNoTracking(Guid id)
    {
        return await _context.Surfs
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundInDBException($"Surf with id {id} not found.");
    }

    private async Task<Substructure> GetSubstructureNoTracking(Guid id)
    {
        return await _context.Substructures
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundInDBException($"Substructure with id {id} not found.");
    }

    private async Task<DrainageStrategy> GetDrainageStrategyNoTracking(Guid id)
    {
        return await _context.DrainageStrategies
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
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id)
        ?? throw new NotFoundInDBException($"DrainageStrategy with id {id} not found.");
    }

    private async Task<Topside> GetTopsideNoTracking(Guid id)
    {
        return await _context.Topsides
                .Include(c => c.CostProfile)
                .Include(c => c.CostProfileOverride)
                .Include(c => c.CessationCostProfile)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundInDBException($"Topside with id {id} not found.");
    }

    private async Task<WellProject> GetWellProjectNoTracking(Guid id)
    {
        return await _context.WellProjects
                .Include(c => c.WellProjectWells)!.ThenInclude(c => c.DrillingSchedule)
                .Include(c => c.OilProducerCostProfile)
                .Include(c => c.OilProducerCostProfileOverride)
                .Include(c => c.GasProducerCostProfile)
                .Include(c => c.GasProducerCostProfileOverride)
                .Include(c => c.WaterInjectorCostProfile)
                .Include(c => c.WaterInjectorCostProfileOverride)
                .Include(c => c.GasInjectorCostProfile)
                .Include(c => c.GasInjectorCostProfileOverride)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundInDBException($"WellProject with id {id} not found.");
    }

    private async Task<Exploration> GetExplorationNoTracking(Guid id)
    {
        return await _context.Explorations
                .Include(c => c.ExplorationWells)!.ThenInclude(c => c.DrillingSchedule)
                .Include(c => c.ExplorationWellCostProfile)
                .Include(c => c.AppraisalWellCostProfile)
                .Include(c => c.SidetrackCostProfile)
                .Include(c => c.GAndGAdminCost)
                .Include(c => c.GAndGAdminCostOverride)
                .Include(c => c.SeismicAcquisitionAndProcessing)
                .Include(c => c.CountryOfficeCost)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundInDBException($"Exploration with id {id} not found.");
    }
}
