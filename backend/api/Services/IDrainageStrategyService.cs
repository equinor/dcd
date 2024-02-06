using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IDrainageStrategyService
    {
        Task<ProjectDto> CreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId);
        Task<DrainageStrategy> NewCreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId);
        Task<DrainageStrategyDto> CopyDrainageStrategy(Guid drainageStrategyId, Guid sourceCaseId);
        Task<ProjectDto> DeleteDrainageStrategy(Guid drainageStrategyId);
        Task<ProjectDto> UpdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto);
        Task<DrainageStrategy> GetDrainageStrategy(Guid drainageStrategyId);
    }
}
