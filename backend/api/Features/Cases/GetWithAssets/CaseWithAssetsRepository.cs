using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.GetWithAssets;

public class CaseWithAssetsRepository(DcdDbContext context)
{
    public async Task<(
        Case CaseItem,
        DrainageStrategy DrainageStrategy,
        Topside Topside,
        Exploration Exploration,
        Substructure Substructure,
        Surf Surf,
        Transport Transport,
        OnshorePowerSupply OnshorePowerSupply,
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
        var onshorePowerSupply = await GetOnshorePowerSupplyNoTracking(caseItem.OnshorePowerSupplyLink);
        var wellProject = await GetWellProjectNoTracking(caseItem.WellProjectLink);

        return (caseItem, drainageStrategy, topside, exploration, substructure, surf, transport, onshorePowerSupply, wellProject);
    }

    private async Task<Case> GetCaseNoTracking(Guid caseId)
    {
        return await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .AsNoTracking()
            .SingleAsync(c => c.Id == caseId);
    }

    private async Task<Transport> GetTransportNoTracking(Guid transportLink)
    {
        return await context.Transports
            .AsNoTracking()
            .SingleAsync(o => o.Id == transportLink);
    }

    private async Task<OnshorePowerSupply> GetOnshorePowerSupplyNoTracking(Guid onshorePowerSupplyLink)
    {
        return await context.OnshorePowerSupplies
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .AsNoTracking()
            .FirstAsync(o => o.Id == onshorePowerSupplyLink);
    }

    private async Task<Surf> GetSurfNoTracking(Guid surfLink)
    {
        return await context.Surfs
            .AsNoTracking()
            .SingleAsync(o => o.Id == surfLink);
    }

    private async Task<Substructure> GetSubstructureNoTracking(Guid substructureLink)
    {
        return await context.Substructures
            .AsNoTracking()
            .SingleAsync(o => o.Id == substructureLink);
    }

    private async Task<DrainageStrategy> GetDrainageStrategyNoTracking(Guid drainageStrategyLink)
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
            .Include(c => c.Co2Intensity)
            .Include(c => c.ProductionProfileNgl)
            .Include(c => c.ImportedElectricity)
            .Include(c => c.ImportedElectricityOverride)
            .Include(c => c.DeferredOilProduction)
            .Include(c => c.DeferredGasProduction)
            .AsNoTracking()
            .SingleAsync(o => o.Id == drainageStrategyLink);
    }

    private async Task<Topside> GetTopsideNoTracking(Guid topsideLink)
    {
        return await context.Topsides
            .AsNoTracking()
            .SingleAsync(o => o.Id == topsideLink);
    }

    private async Task<WellProject> GetWellProjectNoTracking(Guid wellProjectLink)
    {
        return await context.WellProjects
            .Include(c => c.WellProjectWells).ThenInclude(c => c.DrillingSchedule)
            .Include(c => c.OilProducerCostProfile)
            .Include(c => c.OilProducerCostProfileOverride)
            .Include(c => c.GasProducerCostProfile)
            .Include(c => c.GasProducerCostProfileOverride)
            .Include(c => c.WaterInjectorCostProfile)
            .Include(c => c.WaterInjectorCostProfileOverride)
            .Include(c => c.GasInjectorCostProfile)
            .Include(c => c.GasInjectorCostProfileOverride)
            .AsNoTracking()
            .SingleAsync(o => o.Id == wellProjectLink);
    }

    private async Task<Exploration> GetExplorationNoTracking(Guid explorationLink)
    {
        return await context.Explorations
            .Include(c => c.ExplorationWells).ThenInclude(c => c.DrillingSchedule)
            .Include(c => c.ExplorationWellCostProfile)
            .Include(c => c.AppraisalWellCostProfile)
            .Include(c => c.SidetrackCostProfile)
            .Include(c => c.GAndGAdminCost)
            .Include(c => c.GAndGAdminCostOverride)
            .Include(c => c.SeismicAcquisitionAndProcessing)
            .Include(c => c.CountryOfficeCost)
            .AsNoTracking()
            .SingleAsync(o => o.Id == explorationLink);
    }
}
