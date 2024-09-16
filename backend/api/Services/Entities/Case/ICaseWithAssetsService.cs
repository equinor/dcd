using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICaseWithAssetsService
{
    Task<CaseWithAssetsDto> GetCaseWithAssetsNoTracking(Guid projectId, Guid caseId);
}
