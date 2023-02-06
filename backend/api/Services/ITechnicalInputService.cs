using api.Dtos;

namespace api.Services
{
    public interface ITechnicalInputService
    {
        (ExplorationDto explorationDto, WellProjectDto wellProjectDto)? CreateAndUpdateWells(WellDto[] wellDtos, Guid? caseId);
        DevelopmentOperationalWellCostsDto UpdateDevelopmentOperationalWellCosts(DevelopmentOperationalWellCostsDto updatedDto);
        ExplorationOperationalWellCostsDto UpdateExplorationOperationalWellCosts(ExplorationOperationalWellCostsDto updatedDto);
        ProjectDto UpdateProject(ProjectDto updatedDto);
        TechnicalInputDto UpdateTehnicalInput(TechnicalInputDto technicalInputDto);
    }
}
