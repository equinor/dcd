using api.Dtos;
using api.Models;

using static api.Services.CaseWithAssetsService;

namespace api.Services;

public interface ICaseWithAssetsService
{
    Task CreateAndUpdateExplorationWellsAsync(UpdateExplorationWellDto[] explorationWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate);
    Task CreateAndUpdateWellProjectWellsAsync(UpdateWellProjectWellDto[] wellProjectWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate);
    Task<CaseDto> UpdateCase(Guid caseId, UpdateCaseDto updatedDto, ProfilesToGenerate profilesToGenerate);
    Task<ProjectWithGeneratedProfilesDto> UpdateCaseWithAssetsAsync(Guid projectId, Guid caseId, CaseWithAssetsWrapperDto wrapper);
}
