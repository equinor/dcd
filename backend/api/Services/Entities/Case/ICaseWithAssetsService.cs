using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICaseWithAssetsService
{
    Task<CaseWithAssetsDto> GetCaseWithAssets(Guid projectId, Guid caseId);
}
