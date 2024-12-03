namespace api.Features.CaseProfiles.Repositories;

public interface IBaseRepository
{
    Task SaveChangesAndRecalculateAsync(Guid caseId);
}
