using api.Context;
using api.Models;


namespace api.Repositories;

public class WellProjectRepository : BaseRepository, IWellProjectRepository
{

    public WellProjectRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<WellProject?> GetWellProject(Guid wellProjectId)
    {
        return await Get<WellProject>(wellProjectId);
    }

    public WellProject UpdateWellProject(WellProject wellProject)
    {
        return Update(wellProject);
    }

    public async Task<WellProjectWell?> GetWellProjectWell(Guid wellProjectId, Guid wellId)
    {
        return await Get<WellProjectWell>(wellId);
    }

    public  WellProjectWell UpdateWellProjectWell(WellProjectWell wellProjectWell)
    {
        return Update(wellProjectWell);
    }

    public async Task<OilProducerCostProfileOverride?> GetOilProducerCostProfileOverride(Guid profileId)
    {
        return await Get<OilProducerCostProfileOverride>(profileId);
    }

    public OilProducerCostProfileOverride UpdateOilProducerCostProfileOverride(OilProducerCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<GasProducerCostProfileOverride?> GetGasProducerCostProfileOverride(Guid profileId)
    {
        return await Get<GasProducerCostProfileOverride>(profileId);
    }

    public GasProducerCostProfileOverride UpdateGasProducerCostProfileOverride(GasProducerCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<WaterInjectorCostProfileOverride?> GetWaterInjectorCostProfileOverride(Guid profileId)
    {
        return await Get<WaterInjectorCostProfileOverride>(profileId);
    }

    public WaterInjectorCostProfileOverride UpdateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<GasInjectorCostProfileOverride?> GetGasInjectorCostProfileOverride(Guid profileId)
    {
        return await Get<GasInjectorCostProfileOverride>(profileId);
    }

    public GasInjectorCostProfileOverride UpdateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }
}
