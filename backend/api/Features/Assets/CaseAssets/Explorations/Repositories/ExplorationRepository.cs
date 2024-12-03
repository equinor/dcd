using System.Linq.Expressions;

using api.Context;
using api.Features.CaseProfiles.Enums;
using api.Features.CaseProfiles.Repositories;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Explorations.Repositories;

public class ExplorationRepository(DcdDbContext context) : BaseRepository(context), IExplorationRepository
{
    public async Task<Exploration?> GetExploration(Guid explorationId)
    {
        return await Get<Exploration>(explorationId);
    }

    public async Task<Exploration?> GetExplorationWithIncludes(Guid explorationId, params Expression<Func<Exploration, object>>[] includes)
    {
        return await GetWithIncludes(explorationId, includes);
    }

    public async Task<Well?> GetWell(Guid wellId)
    {
        return await Get<Well>(wellId);
    }

    public async Task<bool> ExplorationHasProfile(Guid explorationId, ExplorationProfileNames profileType)
    {
        Expression<Func<Exploration, bool>> profileExistsExpression = profileType switch
        {
            ExplorationProfileNames.GAndGAdminCostOverride => d => d.GAndGAdminCostOverride != null,
            ExplorationProfileNames.SeismicAcquisitionAndProcessing => d => d.SeismicAcquisitionAndProcessing != null,
            ExplorationProfileNames.CountryOfficeCost => d => d.CountryOfficeCost != null,
        };

        bool hasProfile = await Context.Explorations
            .Where(d => d.Id == explorationId)
            .AnyAsync(profileExistsExpression);

        return hasProfile;
    }

    public async Task<DrillingSchedule?> GetExplorationWellDrillingSchedule(Guid drillingScheduleId)
    {
        return await Get<DrillingSchedule>(drillingScheduleId);
    }

    public async Task<Exploration?> GetExplorationWithDrillingSchedule(Guid drillingScheduleId)
    {
        var exploration = await Context.Explorations
            .Include(e => e.ExplorationWells)
            .ThenInclude(w => w.DrillingSchedule)
            .FirstOrDefaultAsync(e => e.ExplorationWells != null && e.ExplorationWells.Any(w => w.DrillingScheduleId == drillingScheduleId));

        return exploration;
    }

    public DrillingSchedule UpdateExplorationWellDrillingSchedule(DrillingSchedule drillingSchedule)
    {
        return Update(drillingSchedule);
    }

    public ExplorationWell CreateExplorationWellDrillingSchedule(ExplorationWell explorationWellWithDrillingSchedule)
    {
        Context.ExplorationWell.Add(explorationWellWithDrillingSchedule);
        return explorationWellWithDrillingSchedule;
    }
}
