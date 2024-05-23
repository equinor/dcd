using api.Models;

namespace api.Repositories;

public interface IWellProjectRepository
{
    Task<WellProject?> GetWellProject(Guid wellProjectId);
    Task<WellProject> UpdateWellProject(WellProject wellProject);
    Task<OilProducerCostProfileOverride?> GetOilProducerCostProfileOverride(Guid profileId);
    Task<OilProducerCostProfileOverride> UpdateOilProducerCostProfileOverride(OilProducerCostProfileOverride costProfile);
    Task<GasProducerCostProfileOverride?> GetGasProducerCostProfileOverride(Guid profileId);
    Task<GasProducerCostProfileOverride> UpdateGasProducerCostProfileOverride(GasProducerCostProfileOverride costProfile);
    Task<WaterInjectorCostProfileOverride?> GetWaterInjectorCostProfileOverride(Guid profileId);
    Task<WaterInjectorCostProfileOverride> UpdateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride costProfile);
    Task<GasInjectorCostProfileOverride?> GetGasInjectorCostProfileOverride(Guid profileId);
    Task<GasInjectorCostProfileOverride> UpdateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride costProfile);
}
