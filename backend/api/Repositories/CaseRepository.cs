using api.Context;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class CaseRepository : ICaseRepository
{
    private readonly DcdDbContext _context;
    private readonly ILogger<CaseRepository> _logger;

    public CaseRepository(
        DcdDbContext context,
        ILogger<CaseRepository> logger
        )
    {
        _context = context;
        _logger = logger;
    }

    public async Task UpdateModifyTime(Guid caseId)
    {
        if (caseId == Guid.Empty)
        {
            throw new ArgumentException("The case id cannot be empty.", nameof(caseId));
        }

        var caseItem = await _context.Cases.SingleOrDefaultAsync(c => c.Id == caseId)
            ?? throw new KeyNotFoundException($"Case with id {caseId} not found.");

        caseItem.ModifyTime = DateTimeOffset.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Failed to update ModifyDate for Case with id {caseId}.", caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "An error occurred while updating ModifyDate for the Case with id {caseId}.", caseId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update modify time for case id {CaseId}, but operation continues.", caseId);
        }
    }
}
