using api.Dtos;

namespace api.Services;

public interface ICaseWithAssetsService
{
    Task<CaseWithAssetsDto> GetCaseWithAssetsNoTracking(Guid projectId, Guid caseId);
}
