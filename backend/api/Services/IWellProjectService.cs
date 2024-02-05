using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IWellProjectService
    {
        Task<WellProjectDto> CopyWellProject(Guid wellProjectId, Guid sourceCaseId);
        Task<ProjectDto> CreateWellProject(WellProject wellProject, Guid sourceCaseId);
        Task<WellProject> NewCreateWellProject(WellProjectDto wellProjectDto, Guid sourceCaseId);
        Task<ProjectDto> DeleteWellProject(Guid wellProjectId);
        Task<ProjectDto> UpdateWellProject(WellProjectDto updatedWellProject);
        Task<WellProjectDto> NewUpdateWellProject(WellProjectDto updatedWellProjectDto);
        Task<WellProjectDto[]> UpdateMultiple(WellProjectDto[] updatedWellProjectDtos);
        Task<WellProjectDto> UpdateSingleWellProject(WellProjectDto updatedWellProjectDto);
        Task<WellProject> GetWellProject(Guid wellProjectId);
    }
}
