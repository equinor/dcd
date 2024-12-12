using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.WellProjects.Repositories;

public interface IWellProjectTimeSeriesRepository
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
