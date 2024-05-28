using api.Context;
using api.Models;


namespace api.Repositories;

public class ExplorationRepository : IExplorationRepository
{
    private readonly DcdDbContext _context;

    public ExplorationRepository(DcdDbContext context)
    {
        _context = context;
    }

    public async Task<Exploration?> GetExploration(Guid explorationId)
    {
        return await _context.Explorations.FindAsync(explorationId);
    }

    public async Task<Exploration> UpdateExploration(Exploration exploration)
    {
        _context.Explorations.Update(exploration);
        await _context.SaveChangesAsync();
        return exploration;
    }

    public async Task<SeismicAcquisitionAndProcessing?> GetSeismicAcquisitionAndProcessing(Guid seismicAcquisitionAndProcessingId)
    {
        return await _context.SeismicAcquisitionAndProcessing.FindAsync(seismicAcquisitionAndProcessingId);
    }

    public async Task<SeismicAcquisitionAndProcessing> UpdateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing seismicAcquisitionAndProcessing)
    {
        _context.SeismicAcquisitionAndProcessing.Update(seismicAcquisitionAndProcessing);
        await _context.SaveChangesAsync();
        return seismicAcquisitionAndProcessing;
    }

    public async Task<CountryOfficeCost?> GetCountryOfficeCost(Guid countryOfficeCostId)
    {
        return await _context.CountryOfficeCost.FindAsync(countryOfficeCostId);
    }

    public async Task<CountryOfficeCost> UpdateCountryOfficeCost(CountryOfficeCost countryOfficeCost)
    {
        _context.CountryOfficeCost.Update(countryOfficeCost);
        await _context.SaveChangesAsync();
        return countryOfficeCost;
    }
}
