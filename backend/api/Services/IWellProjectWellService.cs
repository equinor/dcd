using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IWellProjectWellService
    {
        WellProjectWellDto[]? CopyWellProjectWell(Guid sourceWellProjectId, Guid targetWellProjectId);
        WellProjectWellDto[]? CreateMultipleWellProjectWells(WellProjectWellDto[] wellProjectWellDtos);
        ProjectDto CreateWellProjectWell(WellProjectWellDto wellProjectWellDto);
        IEnumerable<WellProjectWell> GetAll();
        IEnumerable<WellProjectWellDto> GetAllDtos();
        WellProjectWell GetWellProjectWell(Guid wellId, Guid caseId);
        WellProjectWellDto GetWellProjectWellDto(Guid wellId, Guid caseId);
        WellProjectWellDto[]? UpdateMultipleWellProjectWells(WellProjectWellDto[] updatedWellProjectWellDtos, Guid caseId);
        ProjectDto UpdateWellProjectWell(WellProjectWellDto updatedWellProjectWellDto);
    }
}
