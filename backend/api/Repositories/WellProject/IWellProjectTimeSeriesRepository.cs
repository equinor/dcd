using api.Enums;
using api.Models;

namespace api.Repositories;

public interface IWellProjectTimeSeriesRepository : IBaseRepository
{
    OilProducerCostProfileOverride CreateOilProducerCostProfileOverride(OilProducerCostProfileOverride profile);
    GasProducerCostProfileOverride CreateGasProducerCostProfileOverride(GasProducerCostProfileOverride profile);
    WaterInjectorCostProfileOverride CreateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride profile);
    GasInjectorCostProfileOverride CreateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride profile);
    Task<OilProducerCostProfileOverride?> GetOilProducerCostProfileOverride(Guid profileId);
    OilProducerCostProfileOverride UpdateOilProducerCostProfileOverride(OilProducerCostProfileOverride costProfile);
    Task<GasProducerCostProfileOverride?> GetGasProducerCostProfileOverride(Guid profileId);
    GasProducerCostProfileOverride UpdateGasProducerCostProfileOverride(GasProducerCostProfileOverride costProfile);
    Task<WaterInjectorCostProfileOverride?> GetWaterInjectorCostProfileOverride(Guid profileId);
    WaterInjectorCostProfileOverride UpdateWaterInjectorCostProfileOverride(WaterInjectorCostProfileOverride costProfile);
    Task<GasInjectorCostProfileOverride?> GetGasInjectorCostProfileOverride(Guid profileId);
    GasInjectorCostProfileOverride UpdateGasInjectorCostProfileOverride(GasInjectorCostProfileOverride costProfile);
}
