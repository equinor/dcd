using api.Models;

namespace api.Features.CaseProfiles.Services;

public interface ICaseService
{
    Task<Case> GetCase(Guid caseId);
}
