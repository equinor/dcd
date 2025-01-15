using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides.Dtos;

public class UpdateFuelFlaringAndLossesOverrideDto : UpdateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
