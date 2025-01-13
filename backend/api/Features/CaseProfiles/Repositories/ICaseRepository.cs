using api.Features.CaseProfiles.Enums;
using api.Models;

namespace api.Features.CaseProfiles.Repositories;

public interface ICaseRepository
{
    Task<Project> GetProject(Guid projectPk);
    Task<Case?> GetCase(Guid caseId);
    Task<bool> CaseHasProfile(Guid caseId, CaseProfileNames profileType);
    Task UpdateModifyTime(Guid caseId);
}
