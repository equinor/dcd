using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IWellProjectService
    {
        WellProjectDto CopyWellProject(Guid wellProjectId, Guid sourceCaseId);
        ProjectDto CreateWellProject(WellProject wellProject, Guid sourceCaseId);
        ProjectDto DeleteWellProject(Guid wellProjectId);
        WellProject GetWellProject(Guid wellProjectId);
        WellProject NewCreateWellProject(WellProjectDto wellProjectDto, Guid sourceCaseId);
        WellProjectDto NewUpdateWellProject(WellProjectDto updatedWellProjectDto);
        WellProjectDto[] UpdateMultiple(WellProjectDto[] updatedWellProjectDtos);
        WellProjectDto UpdateSingleWellProject(WellProjectDto updatedWellProjectDto);
        ProjectDto UpdateWellProject(WellProjectDto updatedWellProject);
    }
}
