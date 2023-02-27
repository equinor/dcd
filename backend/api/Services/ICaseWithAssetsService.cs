using api.Dtos;
using api.Models;

using static api.Services.CaseWithAssetsService;

namespace api.Services
{
    public interface ICaseWithAssetsService
    {
        Task CreateAndUpdateExplorationWellsAsync(ExplorationWellDto[] explorationWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate);
        Task CreateAndUpdateWellProjectWellsAsync(WellProjectWellDto[] wellProjectWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate);
        CaseDto UpdateCase(CaseDto updatedDto, ProfilesToGenerate profilesToGenerate);
        Task<ProjectWithGeneratedProfilesDto> UpdateCaseWithAssetsAsync(CaseWithAssetsWrapperDto wrapper);
        DrainageStrategyDto UpdateDrainageStrategy(DrainageStrategyDto updatedDto, PhysUnit unit, ProfilesToGenerate profilesToGenerate);
        ExplorationDto UpdateExploration(ExplorationDto updatedDto, ProfilesToGenerate profilesToGenerate);
        SubstructureDto UpdateSubstructure(SubstructureDto updatedDto, ProfilesToGenerate profilesToGenerate);
        SurfDto UpdateSurf(SurfDto updatedDto, ProfilesToGenerate profilesToGenerate);
        TopsideDto UpdateTopside(TopsideDto updatedDto, ProfilesToGenerate profilesToGenerate);
        TransportDto UpdateTransport(TransportDto updatedDto, ProfilesToGenerate profilesToGenerate);
        WellProjectDto UpdateWellProject(WellProjectDto updatedDto, ProfilesToGenerate profilesToGenerate);
    }
}
