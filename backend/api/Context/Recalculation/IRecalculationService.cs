namespace api.Context.Recalculation;

public interface IRecalculationService
{
    Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default);
}
