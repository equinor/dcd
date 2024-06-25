using System.Linq.Expressions;

using api.Context;
using api.Enums;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class ExplorationRepository : BaseRepository, IExplorationRepository
{

    public ExplorationRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<Exploration?> GetExploration(Guid explorationId)
    {
        return await Get<Exploration>(explorationId);
    }

    public async Task<Well?> GetWell(Guid wellId)
    {
        return await Get<Well>(wellId);
    }

    public async Task<bool> ExplorationHasProfile(Guid ExplorationId, ExplorationProfileNames profileType)
    {
        Expression<Func<Exploration, bool>> profileExistsExpression = profileType switch
        {
            ExplorationProfileNames.SeismicAcquisitionAndProcessing => d => d.SeismicAcquisitionAndProcessing != null,
            ExplorationProfileNames.CountryOfficeCost => d => d.CountryOfficeCost != null,
        };

        bool hasProfile = await _context.Explorations
            .Where(d => d.Id == ExplorationId)
            .AnyAsync(profileExistsExpression);

        return hasProfile;
    }

    public Exploration UpdateExploration(Exploration exploration)
    {
        return Update(exploration);
    }

    public async Task<ExplorationWell?> GetExplorationWell(Guid explorationId, Guid wellId)
    {
        return await _context.ExplorationWell.FindAsync(explorationId, wellId);
    }

    public async Task<DrillingSchedule?> GetExplorationWellDrillingSchedule(Guid drillingScheduleId)
    {
        return await Get<DrillingSchedule>(drillingScheduleId);
    }

    public DrillingSchedule UpdateExplorationWellDrillingSchedule(DrillingSchedule drillingSchedule)
    {
        return Update(drillingSchedule);
    }

    public ExplorationWell CreateExplorationWellDrillingSchedule(ExplorationWell explorationWellWithDrillingSchedule)
    {
        _context.ExplorationWell.Add(explorationWellWithDrillingSchedule);
        return explorationWellWithDrillingSchedule;
    }
}
