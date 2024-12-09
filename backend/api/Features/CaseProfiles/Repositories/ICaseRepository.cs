using System.Linq.Expressions;

using api.Features.CaseProfiles.Enums;
using api.Models;

namespace api.Features.CaseProfiles.Repositories;


public interface ICaseRepository : IBaseRepository
{
    Task<Project> GetProject(Guid projectPk);
    Task<Case?> GetCase(Guid caseId);
    Task<Case?> GetCaseWithIncludes(Guid caseId, params Expression<Func<Case, object>>[] includes);
    Task<bool> CaseHasProfile(Guid caseId, CaseProfileNames profileType);
    Task UpdateModifyTime(Guid caseId);
    Task<Guid> GetPrimaryKeyForProjectId(Guid projectId);
}
