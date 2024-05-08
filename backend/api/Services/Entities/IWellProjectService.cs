using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectService
{
    Task<WellProjectDto> CopyWellProject(Guid wellProjectId, Guid sourceCaseId);
    Task<WellProject> CreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto);
    Task<WellProjectDto> UpdateWellProject(WellProjectDto updatedWellProjectDto);
    Task<WellProject> GetWellProject(Guid wellProjectId);
}
