using System.Linq.Expressions;

using api.Context;
using api.Enums;
using api.Models;

using Microsoft.EntityFrameworkCore;


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
    public async Task<bool> WellProjectHasProfile(Guid WellProjectId, WellProjectProfileNames profileType)
    {
        Expression<Func<WellProject, bool>> profileExistsExpression = profileType switch
        {
            WellProjectProfileNames.OilProducerCostProfileOverride => d => d.OilProducerCostProfileOverride != null,
            WellProjectProfileNames.GasProducerCostProfileOverride => d => d.GasProducerCostProfileOverride != null,
            WellProjectProfileNames.WaterInjectorCostProfileOverride => d => d.WaterInjectorCostProfileOverride != null,
            WellProjectProfileNames.GasInjectorCostProfileOverride => d => d.GasInjectorCostProfileOverride != null,
        };

        bool hasProfile = await _context.WellProjects
            .Where(d => d.Id == WellProjectId)
            .AnyAsync(profileExistsExpression);

        return hasProfile;
    }

    public WellProject UpdateWellProject(WellProject wellProject)
    {
        return Update(wellProject);
    }

    public async Task<WellProjectWell?> GetWellProjectWell(Guid wellProjectId, Guid wellId)
    {
        return await Get<WellProjectWell>(wellId);
    }

    public WellProjectWell UpdateWellProjectWell(WellProjectWell wellProjectWell)
    {
        return Update(wellProjectWell);
    }

    public OilProducerCostProfileOverride CreateOilProducerCostProfileOverride(OilProducerCostProfileOverride profile)
    {
        _context.OilProducerCostProfileOverride.Add(profile);
        return profile;
    }

    public GasProducerCostProfileOverride CreateGasProducerCostProfileOverride(GasProducerCostProfileOverride profile)
    {
        _context.GasProducerCostProfileOverride.Add(profile);
        return profile;
    }

    public WaterInjectorCostProfileOverride CreateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride profile)
    {
        _context.WaterInjectorCostProfileOverride.Add(profile);
        return profile;
    }

    public GasInjectorCostProfileOverride CreateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride profile)
    {
        _context.GasInjectorCostProfileOverride.Add(profile);
        return profile;
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
