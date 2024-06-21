using api.Dtos;
using api.Models;

using static api.Services.CaseAndAssetsService;

namespace api.Services;

// TODO: Delete once autosave is implemented
public interface ICaseAndAssetsService
{
    Task<ProjectWithGeneratedProfilesDto> UpdateCaseWithAssets(Guid projectId, Guid caseId, CaseWithAssetsWrapperDto wrapper);
}
