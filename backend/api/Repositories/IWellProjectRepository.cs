using api.Models;

namespace api.Repositories;

public interface IWellProjectRepository : IBaseRepository
{
    Task<WellProject?> GetWellProject(Guid wellProjectId);
    WellProject UpdateWellProject(WellProject wellProject);
    Task<WellProjectWell?> GetWellProjectWell(Guid wellProjectId, Guid wellId);
    WellProjectWell UpdateWellProjectWell(WellProjectWell wellProjectWell);
    Task<OilProducerCostProfileOverride?> GetOilProducerCostProfileOverride(Guid profileId);
    OilProducerCostProfileOverride UpdateOilProducerCostProfileOverride(OilProducerCostProfileOverride costProfile);
    Task<GasProducerCostProfileOverride?> GetGasProducerCostProfileOverride(Guid profileId);
    GasProducerCostProfileOverride UpdateGasProducerCostProfileOverride(GasProducerCostProfileOverride costProfile);
    Task<WaterInjectorCostProfileOverride?> GetWaterInjectorCostProfileOverride(Guid profileId);
    WaterInjectorCostProfileOverride UpdateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride costProfile);
    Task<GasInjectorCostProfileOverride?> GetGasInjectorCostProfileOverride(Guid profileId);
    GasInjectorCostProfileOverride UpdateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride costProfile);
}
