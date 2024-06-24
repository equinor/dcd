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

    public async Task<SeismicAcquisitionAndProcessing?> GetSeismicAcquisitionAndProcessing(Guid seismicAcquisitionAndProcessingId)
    {
        return await Get<SeismicAcquisitionAndProcessing>(seismicAcquisitionAndProcessingId);
    }

    public SeismicAcquisitionAndProcessing CreateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing profile)
    {
        _context.SeismicAcquisitionAndProcessing.Add(profile);
        return profile;
    }

    public CountryOfficeCost CreateCountryOfficeCost(CountryOfficeCost profile)
    {
        _context.CountryOfficeCost.Add(profile);
        return profile;
    }

    public SeismicAcquisitionAndProcessing UpdateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing seismicAcquisitionAndProcessing)
    {
        return Update(seismicAcquisitionAndProcessing);
    }

    public async Task<CountryOfficeCost?> GetCountryOfficeCost(Guid countryOfficeCostId)
    {
        return await Get<CountryOfficeCost>(countryOfficeCostId);
    }

    public CountryOfficeCost UpdateCountryOfficeCost(CountryOfficeCost countryOfficeCost)
    {
        return Update(countryOfficeCost);
    }
}
