using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectService
{
    Task<WellProject> CreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto);
    Task<WellProject> GetWellProject(Guid wellProjectId);
    Task<WellProjectDto> UpdateWellProject(
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto
    );
    Task<WellProjectWellDto> UpdateWellProjectWell(
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        UpdateWellProjectWellDto updatedWellProjectWellDto
    );
}
