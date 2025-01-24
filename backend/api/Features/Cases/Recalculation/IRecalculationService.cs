namespace api.Features.Cases.Recalculation;

public interface IRecalculationService
{
    Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, long>> RunAllRecalculations(Guid caseId);
}
