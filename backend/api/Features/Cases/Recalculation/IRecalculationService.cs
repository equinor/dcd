namespace api.Features.Cases.Recalculation;

public interface IRecalculationService
{
    Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, Dictionary<string, long>>> RunAllRecalculations(List<Guid> caseIds);
}
