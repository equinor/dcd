namespace api.Dtos;

public class CreateProductionProfileOilDto : CreateTimeSeriesVolumeDto
{
}

public class CreateProductionProfileGasDto : CreateTimeSeriesVolumeDto
{
}

public class CreateProductionProfileWaterDto : CreateTimeSeriesVolumeDto
{
}

public class CreateProductionProfileWaterInjectionDto : CreateTimeSeriesVolumeDto
{
}

public class CreateFuelFlaringAndLossesOverrideDto : CreateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
public class CreateNetSalesGasOverrideDto : CreateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateCo2EmissionsOverrideDto : CreateTimeSeriesMassDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateImportedElectricityOverrideDto : CreateTimeSeriesEnergyDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateDeferredOilProductionDto : CreateTimeSeriesVolumeDto
{
}

public class CreateDeferredGasProductionDto : CreateTimeSeriesVolumeDto
{
}
