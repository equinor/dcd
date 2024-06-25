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

    public async Task<Well?> GetWell(Guid wellId)
    {
        return await Get<Well>(wellId);
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

    public async Task<DrillingSchedule?> GetWellProjectWellDrillingSchedule(Guid drillingScheduleId)
    {
        return await Get<DrillingSchedule>(drillingScheduleId);
    }

    public DrillingSchedule UpdateWellProjectWellDrillingSchedule(DrillingSchedule drillingSchedule)
    {
        return Update(drillingSchedule);
    }

    public WellProjectWell CreateWellProjectWellDrillingSchedule(WellProjectWell wellProjectWellWithDrillingSchedule)
    {
        _context.WellProjectWell.Add(wellProjectWellWithDrillingSchedule);
        return wellProjectWellWithDrillingSchedule;
    }
}
