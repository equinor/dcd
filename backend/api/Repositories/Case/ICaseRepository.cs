using System.Linq.Expressions;

using api.Enums;
using api.Models;

namespace api.Repositories;


public interface ICaseRepository : IBaseRepository
{
    Task<Case?> GetCase(Guid caseId);
    Task<Case?> GetCaseWithIncludes(Guid caseId, params Expression<Func<Case, object>>[] includes);
    Case UpdateCase(Case updatedCase);
    Task<bool> CaseHasProfile(Guid caseId, CaseProfileNames profileType);
    Task UpdateModifyTime(Guid caseId);
}
