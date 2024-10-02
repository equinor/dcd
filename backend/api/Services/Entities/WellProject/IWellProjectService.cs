using System.Linq.Expressions;

using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectService
{
    Task<WellProject> GetWellProjectWithIncludes(Guid wellProjectId, params Expression<Func<WellProject, object>>[] includes);
    Task<WellProjectDto> UpdateWellProject(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto
    );
    Task<DrillingScheduleDto> UpdateWellProjectWellDrillingSchedule(
        Guid projectId,
            Guid caseId,
            Guid wellProjectId,
            Guid wellId,
            Guid drillingScheduleId,
            UpdateDrillingScheduleDto updatedWellProjectWellDto
        );

    Task<DrillingScheduleDto> CreateWellProjectWellDrillingSchedule(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        CreateDrillingScheduleDto updatedWellProjectWellDto
    );
}
