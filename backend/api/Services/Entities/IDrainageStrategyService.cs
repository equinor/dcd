using api.Dtos;
using api.Models;

namespace api.Services;

public interface IDrainageStrategyService
{
    Task<ProjectDto> CreateDrainageStrategy(DrainageStrategyWithProfilesDto drainageStrategyDto, Guid sourceCaseId);
    Task<DrainageStrategy> NewCreateDrainageStrategy(Guid projectId, Guid sourceCaseId, CreateDrainageStrategyDto drainageStrategyDto);
    Task<DrainageStrategyWithProfilesDto> CopyDrainageStrategy(Guid drainageStrategyId, Guid sourceCaseId);
    Task<DrainageStrategy> GetDrainageStrategy(Guid drainageStrategyId);
    Task<DrainageStrategyDto> UpdateDrainageStrategy(Guid projectId, Guid caseId, Guid drainageStrategyId, UpdateDrainageStrategyDto updatedDrainageStrategyDto);
    Task<ProductionProfileOilDto> UpdateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileOilId,
        UpdateProductionProfileOilDto updatedProductionProfileOilDto
    );

    Task<ProductionProfileGasDto> UpdateProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateProductionProfileGasDto updatedProductionProfileGasDto
    );

    Task<ProductionProfileWaterDto> UpdateProductionProfileWater(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateProductionProfileWaterDto updatedProductionProfileWaterDto
    );

    Task<ProductionProfileWaterInjectionDto> UpdateProductionProfileWaterInjection(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateProductionProfileWaterInjectionDto updatedProductionProfileWaterInjectionDto
    );
}
