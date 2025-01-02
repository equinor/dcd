using System.Linq.Expressions;

using api.Models;

namespace api.Features.CaseProfiles.Services;

public interface ICaseService
{
    Task<Project> GetProject(Guid id);
    Task<Case> GetCase(Guid caseId);
    Task<Case> GetCaseWithIncludes(Guid caseId, params Expression<Func<Case, object>>[] includes);
}
