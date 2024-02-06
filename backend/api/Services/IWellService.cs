using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IWellService
    {
        Task<ProjectDto> CreateWell(WellDto wellDto);
        Task<ProjectDto> DeleteWell(Guid wellId);
        Task<WellDto> UpdateExistingWell(WellDto updatedWellDto);
        Task<Well> GetWell(Guid wellId);
        Task<IEnumerable<Well>> GetAll();
        Task<IEnumerable<Well>> GetWells(Guid projectId);
    }
}
