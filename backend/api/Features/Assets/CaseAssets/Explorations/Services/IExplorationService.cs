using System.Linq.Expressions;

using api.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Models;

namespace api.Features.Assets.CaseAssets.Explorations.Services;

public interface IExplorationService
{
    Task<Exploration> GetExplorationWithIncludes(Guid explorationId, params Expression<Func<Exploration, object>>[] includes);

    Task<ExplorationDto> UpdateExploration(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto
    );

    Task<DrillingScheduleDto> UpdateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        Guid drillingScheduleId,
        UpdateDrillingScheduleDto updatedExplorationWellDto
    );

    Task<DrillingScheduleDto> CreateExplorationWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        CreateDrillingScheduleDto updatedExplorationWellDto
    );
}
