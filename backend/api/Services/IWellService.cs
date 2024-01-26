using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IWellService
    {
        WellDto[]? CreateMultipleWells(WellDto[] wellDtos);
        ProjectDto CreateWell(WellDto wellDto);
        ProjectDto DeleteWell(Guid wellId);

        IEnumerable<Well> GetAll();
        IEnumerable<WellDto> GetAllDtos();
        IEnumerable<WellDto> GetDtosForProject(Guid projectId);
        Well GetWell(Guid wellId);

        WellDto GetWellDto(Guid wellId);
        WellDto UpdateExistingWell(WellDto updatedWellDto);
        WellDto[] UpdateMultipleWells(WellDto[] updatedWellDtos);
        ProjectDto UpdateWell(WellDto updatedWellDto);
    }
}
