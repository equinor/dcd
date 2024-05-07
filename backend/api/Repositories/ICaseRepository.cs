namespace api.Repositories;


public interface ICaseRepository
{
    Task UpdateModifyTime(Guid caseId);
}
