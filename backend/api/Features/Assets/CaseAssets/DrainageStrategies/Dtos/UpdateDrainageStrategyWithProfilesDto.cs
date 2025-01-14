using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;

public class UpdateProductionProfileOilDto : UpdateTimeSeriesVolumeDto;

public class UpdateAdditionalProductionProfileOilDto : UpdateTimeSeriesVolumeDto;

public class UpdateProductionProfileGasDto : UpdateTimeSeriesVolumeDto;

public class UpdateAdditionalProductionProfileGasDto : UpdateTimeSeriesVolumeDto;

public class UpdateProductionProfileWaterDto : UpdateTimeSeriesVolumeDto;

public class UpdateProductionProfileWaterInjectionDto : UpdateTimeSeriesVolumeDto;

public class UpdateFuelFlaringAndLossesOverrideDto : UpdateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
public class UpdateNetSalesGasOverrideDto : UpdateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateCo2EmissionsOverrideDto : UpdateTimeSeriesMassDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateImportedElectricityOverrideDto : UpdateTimeSeriesEnergyDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateDeferredOilProductionDto : UpdateTimeSeriesVolumeDto;

public class UpdateDeferredGasProductionDto : UpdateTimeSeriesVolumeDto;
