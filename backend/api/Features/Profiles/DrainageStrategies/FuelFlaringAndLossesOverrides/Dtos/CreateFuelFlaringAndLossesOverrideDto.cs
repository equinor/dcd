using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides.Dtos;

public class CreateFuelFlaringAndLossesOverrideDto : CreateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
