using api.Context;
using api.Models;


namespace api.Repositories;

public class WellProjectRepository : IWellProjectRepository
{
    private readonly DcdDbContext _context;

    public WellProjectRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<WellProject?> GetWellProject(Guid wellProjectId)
    {
        return await _context.WellProjects.FindAsync(wellProjectId);
    }

    public async Task<WellProject> UpdateWellProject(WellProject wellProject)
    {
        _context.WellProjects.Update(wellProject);
        await _context.SaveChangesAsync();
        return wellProject;
    }

    public async Task<OilProducerCostProfileOverride?> GetOilProducerCostProfileOverride(Guid profileId)
    {
        return await _context.OilProducerCostProfileOverride.FindAsync(profileId);
    }

    public async Task<OilProducerCostProfileOverride> UpdateOilProducerCostProfileOverride(OilProducerCostProfileOverride costProfile)
    {
        _context.OilProducerCostProfileOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<GasProducerCostProfileOverride?> GetGasProducerCostProfileOverride(Guid profileId)
    {
        return await _context.GasProducerCostProfileOverride.FindAsync(profileId);
    }

    public async Task<GasProducerCostProfileOverride> UpdateGasProducerCostProfileOverride(GasProducerCostProfileOverride costProfile)
    {
        _context.GasProducerCostProfileOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<WaterInjectorCostProfileOverride?> GetWaterInjectorCostProfileOverride(Guid profileId)
    {
        return await _context.WaterInjectorCostProfileOverride.FindAsync(profileId);
    }

    public async Task<WaterInjectorCostProfileOverride> UpdateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride costProfile)
    {
        _context.WaterInjectorCostProfileOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }

    public async Task<GasInjectorCostProfileOverride?> GetGasInjectorCostProfileOverride(Guid profileId)
    {
        return await _context.GasInjectorCostProfileOverride.FindAsync(profileId);
    }

    public async Task<GasInjectorCostProfileOverride> UpdateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride costProfile)
    {
        _context.GasInjectorCostProfileOverride.Update(costProfile);
        await _context.SaveChangesAsync();
        return costProfile;
    }
}
