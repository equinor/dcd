namespace api.Repositories;

public interface IBaseRepository
{
    Task SaveChangesAndRecalculateAsync(Guid caseId);
    Task SaveChangesAsync();
}
