using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IWellService
    {
        Task<ProjectDto> CreateWell(WellDto wellDto);
        Task<ProjectDto> UpdateWell(WellDto updatedWellDto);
        Task<ProjectDto> DeleteWell(Guid wellId);
        Task<WellDto> UpdateExistingWell(WellDto updatedWellDto);
        Task<WellDto[]> UpdateMultiple(WellDto[] updatedWellDtos);
        Task<WellDto[]?> CreateMultipleWells(WellDto[] wellDtos);
        Task<Well> GetWell(Guid wellId);
        Task<WellDto> GetWellDto(Guid wellId);
        Task<IEnumerable<Well>> GetAll();
        Task<IEnumerable<WellDto>> GetDtosForProject(Guid projectId);
        Task<IEnumerable<Well>> GetWells(Guid projectId);
        Task<IEnumerable<WellDto>> GetAllDtos();
    }
}
