namespace api.Features.Cases.Recalculation;

public interface IRecalculationService
{
    Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default);
}
