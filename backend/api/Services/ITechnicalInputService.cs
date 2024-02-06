using api.Dtos;

namespace api.Services;

public interface ITechnicalInputService
{
    Task<TechnicalInputDto> UpdateTehnicalInput(TechnicalInputDto technicalInputDto);
    Task<(ExplorationDto explorationDto, WellProjectDto wellProjectDto)?> CreateAndUpdateWells(WellDto[] wellDtos, Guid? caseId);
    Task<ProjectDto> UpdateProject(ProjectDto updatedDto);
    Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts(ExplorationOperationalWellCostsDto updatedDto);
    Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts(DevelopmentOperationalWellCostsDto updatedDto);
}
