using api.Models;

namespace api.Repositories;


public interface ICaseRepository
{
    Task<Case?> GetCase(Guid caseId);
    Task<Case> UpdateCase(Case updatedCase);
    Task UpdateModifyTime(Guid caseId);
}
