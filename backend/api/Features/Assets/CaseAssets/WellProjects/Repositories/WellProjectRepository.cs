using System.Linq.Expressions;

using api.Context;
using api.Features.CaseProfiles.Enums;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.WellProjects.Repositories;

public class WellProjectRepository(DcdDbContext context) : BaseRepository(context), IWellProjectRepository
{
    public async Task<WellProject?> GetWellProject(Guid wellProjectId)
    {
        return await Get<WellProject>(wellProjectId);
    }

    public async Task<Well?> GetWell(Guid wellId)
    {
        return await Get<Well>(wellId);
    }

    public async Task<bool> WellProjectHasProfile(Guid wellProjectId, WellProjectProfileNames profileType)
    {
        Expression<Func<WellProject, bool>> profileExistsExpression = profileType switch
        {
            WellProjectProfileNames.OilProducerCostProfileOverride => d => d.OilProducerCostProfileOverride != null,
            WellProjectProfileNames.GasProducerCostProfileOverride => d => d.GasProducerCostProfileOverride != null,
            WellProjectProfileNames.WaterInjectorCostProfileOverride => d => d.WaterInjectorCostProfileOverride != null,
            WellProjectProfileNames.GasInjectorCostProfileOverride => d => d.GasInjectorCostProfileOverride != null,
        };

        bool hasProfile = await Context.WellProjects
            .Where(d => d.Id == wellProjectId)
            .AnyAsync(profileExistsExpression);

        return hasProfile;
    }

    public async Task<WellProject?> GetWellProjectWithDrillingSchedule(Guid drillingScheduleId)
    {
        var wellProject = await Context.WellProjects
            .Include(e => e.WellProjectWells)
            .ThenInclude(w => w.DrillingSchedule)
            .FirstOrDefaultAsync(e => e.WellProjectWells != null && e.WellProjectWells.Any(w => w.DrillingScheduleId == drillingScheduleId));

        return wellProject;
    }

    public WellProjectWell CreateWellProjectWellDrillingSchedule(WellProjectWell wellProjectWellWithDrillingSchedule)
    {
        Context.WellProjectWell.Add(wellProjectWellWithDrillingSchedule);
        return wellProjectWellWithDrillingSchedule;
    }
}
