using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IWellProjectWellService
    {
        Task<ProjectDto> CreateWellProjectWell(WellProjectWellDto wellProjectWellDto);
        Task<WellProjectWellDto[]?> CreateMultipleWellProjectWells(WellProjectWellDto[] wellProjectWellDtos);
        Task<ProjectDto> UpdateWellProjectWell(WellProjectWellDto updatedWellProjectWellDto);
        Task<WellProjectWellDto[]?> UpdateMultipleWellProjectWells(WellProjectWellDto[] updatedWellProjectWellDtos, Guid caseId);
        Task<WellProjectWell> GetWellProjectWell(Guid wellId, Guid caseId);
        Task<WellProjectWellDto[]?> CopyWellProjectWell(Guid sourceWellProjectId, Guid targetWellProjectId);
        Task<IEnumerable<WellProjectWell>> GetAll();
        Task<IEnumerable<WellProjectWellDto>> GetAllDtos();
    }
}
