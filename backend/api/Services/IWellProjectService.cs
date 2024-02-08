using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectService
{
    Task<WellProjectDto> CopyWellProject(Guid wellProjectId, Guid sourceCaseId);
    Task<WellProject> NewCreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto);
    Task<WellProjectDto> NewUpdateWellProject(WellProjectDto updatedWellProjectDto);
    Task<WellProjectDto[]> UpdateMultiple(WellProjectDto[] updatedWellProjectDtos);
    Task<WellProjectDto> UpdateSingleWellProject(WellProjectDto updatedWellProjectDto);
    Task<WellProject> GetWellProject(Guid wellProjectId);
}
