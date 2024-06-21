using api.Dtos;
using api.Models;

namespace api.Services;

public interface IDrainageStrategyTimeSeriesService
{
    Task<ProductionProfileOilDto> CreateProductionProfileOil(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateProductionProfileOilDto createProductionProfileOilDto);
    Task<ProductionProfileOilDto> UpdateProductionProfileOil(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid productionProfileOilId, UpdateProductionProfileOilDto updatedProductionProfileOilDto);
    Task<ProductionProfileGasDto> CreateProductionProfileGas(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateProductionProfileGasDto createProfileDto);
    Task<ProductionProfileGasDto> UpdateProductionProfileGas(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid productionProfileId, UpdateProductionProfileGasDto updatedProductionProfileGasDto);
    Task<ProductionProfileWaterDto> CreateProductionProfileWater(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateProductionProfileWaterDto createProfileDto);
    Task<ProductionProfileWaterDto> UpdateProductionProfileWater(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid productionProfileId, UpdateProductionProfileWaterDto updatedProductionProfileWaterDto);
    Task<ProductionProfileWaterInjectionDto> CreateProductionProfileWaterInjection(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateProductionProfileWaterInjectionDto createProfileDto);
    Task<ProductionProfileWaterInjectionDto> UpdateProductionProfileWaterInjection(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid productionProfileId, UpdateProductionProfileWaterInjectionDto updatedProductionProfileWaterInjectionDto);
    Task<FuelFlaringAndLossesOverrideDto> CreateFuelFlaringAndLossesOverride(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateFuelFlaringAndLossesOverrideDto createProfileDto);
    Task<FuelFlaringAndLossesOverrideDto> UpdateFuelFlaringAndLossesOverride(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid profileId, UpdateFuelFlaringAndLossesOverrideDto updateDto);
    Task<NetSalesGasOverrideDto> CreateNetSalesGasOverride(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateNetSalesGasOverrideDto createProfileDto);
    Task<NetSalesGasOverrideDto> UpdateNetSalesGasOverride(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid profileId, UpdateNetSalesGasOverrideDto updateDto);
    Task<Co2EmissionsOverrideDto> CreateCo2EmissionsOverride(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateCo2EmissionsOverrideDto createProfileDto);
    Task<Co2EmissionsOverrideDto> UpdateCo2EmissionsOverride(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid profileId, UpdateCo2EmissionsOverrideDto updateDto);
    Task<ImportedElectricityOverrideDto> CreateImportedElectricityOverride(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateImportedElectricityOverrideDto createProfileDto);
    Task<ImportedElectricityOverrideDto> UpdateImportedElectricityOverride(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid profileId, UpdateImportedElectricityOverrideDto updateDto);
    Task<DeferredOilProductionDto> CreateDeferredOilProduction(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateDeferredOilProductionDto createProfileDto);
    Task<DeferredOilProductionDto> UpdateDeferredOilProduction(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid productionProfileId, UpdateDeferredOilProductionDto updatedDeferredOilProductionDto);
    Task<DeferredGasProductionDto> CreateDeferredGasProduction(Guid projectId, Guid caseId, Guid drainageStrategyId, CreateDeferredGasProductionDto createProfileDto);
    Task<DeferredGasProductionDto> UpdateDeferredGasProduction(Guid projectId, Guid caseId, Guid drainageStrategyId, Guid productionProfileId, UpdateDeferredGasProductionDto updatedDeferredGasProductionDto);
}
