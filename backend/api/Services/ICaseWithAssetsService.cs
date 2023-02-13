using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ICaseWithAssetsService
    {
        void CreateAndUpdateExplorationWells(ExplorationWellDto[] explorationWellDtos, Guid caseId);
        void CreateAndUpdateWellProjectWells(WellProjectWellDto[] wellProjectWellDtos, Guid caseId);
        CaseDto UpdateCase(CaseDto updatedDto);
        Task<ProjectDto> UpdateCaseWithAssetsAsync(CaseWithAssetsWrapperDto wrapper);
        DrainageStrategyDto UpdateDrainageStrategy(DrainageStrategyDto updatedDto, PhysUnit unit);
        ExplorationDto UpdateExploration(ExplorationDto updatedDto);
        SubstructureDto UpdateSubstructure(SubstructureDto updatedDto);
        SurfDto UpdateSurf(SurfDto updatedDto);
        TopsideDto UpdateTopside(TopsideDto updatedDto);
        TransportDto UpdateTransport(TransportDto updatedDto);
        WellProjectDto UpdateWellProject(WellProjectDto updatedDto);
    }
}
