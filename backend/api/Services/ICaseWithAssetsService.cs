using api.Dtos;
using api.Models;

using static api.Services.CaseWithAssetsService;

namespace api.Services;

public interface ICaseWithAssetsService
{
    Task CreateAndUpdateExplorationWellsAsync(ExplorationWellDto[] explorationWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate);
    Task CreateAndUpdateWellProjectWellsAsync(WellProjectWellDto[] wellProjectWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate);
    Task<CaseDto> UpdateCase(CaseDto updatedDto, ProfilesToGenerate profilesToGenerate);
    Task<ProjectWithGeneratedProfilesDto> UpdateCaseWithAssetsAsync(CaseWithAssetsWrapperDto wrapper);
    Task<DrainageStrategyDto> UpdateDrainageStrategy(DrainageStrategyDto updatedDto, PhysUnit unit, ProfilesToGenerate profilesToGenerate);
    Task<ExplorationDto> UpdateExploration(ExplorationDto updatedDto, ProfilesToGenerate profilesToGenerate);
    Task<SubstructureDto> UpdateSubstructure(SubstructureDto updatedDto, ProfilesToGenerate profilesToGenerate);
    Task<SurfDto> UpdateSurf(SurfDto updatedDto, ProfilesToGenerate profilesToGenerate);
    Task<TopsideDto> UpdateTopside(TopsideDto updatedDto, ProfilesToGenerate profilesToGenerate);
    Task<TransportDto> UpdateTransport(TransportDto updatedDto, ProfilesToGenerate profilesToGenerate);
    Task<WellProjectDto> UpdateWellProject(WellProjectDto updatedDto, ProfilesToGenerate profilesToGenerate);
}