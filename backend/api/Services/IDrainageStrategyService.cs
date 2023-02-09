using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IDrainageStrategyService
    {
        DrainageStrategyDto CopyDrainageStrategy(Guid drainageStrategyId, Guid sourceCaseId);
        ProjectDto CreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId);
        ProjectDto DeleteDrainageStrategy(Guid drainageStrategyId);
        DrainageStrategy GetDrainageStrategy(Guid drainageStrategyId);
        DrainageStrategy NewCreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId);
        DrainageStrategyDto NewUpdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto);
        ProjectDto UpdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto);
    }
}
