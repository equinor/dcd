using api.Dtos;
using api.Models;

using static api.Services.CaseAndAssetsService;

namespace api.Services;

public interface ICaseWithAssetsService
{
    Task<CaseWithAssetsDto> GetCaseWithAssets(Guid projectId, Guid caseId);
}
