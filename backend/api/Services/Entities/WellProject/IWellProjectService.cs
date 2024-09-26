using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectService
{
    Task<WellProject> CreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto);
    Task<WellProject> GetWellProject(Guid wellProjectId);
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
