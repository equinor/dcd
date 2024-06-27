using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationService
{
    Task<Exploration> CreateExploration(Guid projectId, Guid sourceCaseId, CreateExplorationDto explorationDto);
    Task<Exploration> GetExploration(Guid explorationId);

    Task<ExplorationDto> UpdateExploration(
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto
    );

    Task<DrillingScheduleDto> UpdateExplorationWellDrillingSchedule(
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedExplorationWellDto
    );

    Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        CreateDrillingScheduleDto updatedExplorationWellDto
    );
}
