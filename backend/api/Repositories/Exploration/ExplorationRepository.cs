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

    public ExplorationWell UpdateExplorationWell(ExplorationWell explorationWell)
    {
        return Update(explorationWell);
    }
}
