using System.Linq.Expressions;

using api.Context;
using api.Enums;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class WellProjectTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), IWellProjectTimeSeriesRepository
{
    public OilProducerCostProfileOverride CreateOilProducerCostProfileOverride(OilProducerCostProfileOverride profile)
    {
        Context.OilProducerCostProfileOverride.Add(profile);
        return profile;
    }

    public GasProducerCostProfileOverride CreateGasProducerCostProfileOverride(GasProducerCostProfileOverride profile)
    {
        Context.GasProducerCostProfileOverride.Add(profile);
        return profile;
    }

    public WaterInjectorCostProfileOverride CreateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride profile)
    {
        Context.WaterInjectorCostProfileOverride.Add(profile);
        return profile;
    }

    public GasInjectorCostProfileOverride CreateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride profile)
    {
        Context.GasInjectorCostProfileOverride.Add(profile);
        return profile;
    }

    public async Task<OilProducerCostProfileOverride?> GetOilProducerCostProfileOverride(Guid profileId)
    {
        return await GetWithIncludes<OilProducerCostProfileOverride>(profileId, w => w.WellProject);
    }

    public OilProducerCostProfileOverride UpdateOilProducerCostProfileOverride(OilProducerCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<GasProducerCostProfileOverride?> GetGasProducerCostProfileOverride(Guid profileId)
    {
        return await GetWithIncludes<GasProducerCostProfileOverride>(profileId, w => w.WellProject);
    }

    public GasProducerCostProfileOverride UpdateGasProducerCostProfileOverride(GasProducerCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<WaterInjectorCostProfileOverride?> GetWaterInjectorCostProfileOverride(Guid profileId)
    {
        return await GetWithIncludes<WaterInjectorCostProfileOverride>(profileId, w => w.WellProject);
    }

    public WaterInjectorCostProfileOverride UpdateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<GasInjectorCostProfileOverride?> GetGasInjectorCostProfileOverride(Guid profileId)
    {
        return await GetWithIncludes<GasInjectorCostProfileOverride>(profileId, w => w.WellProject);
    }

    public GasInjectorCostProfileOverride UpdateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride costProfile)
    {
        return Update(costProfile);
    }
}
