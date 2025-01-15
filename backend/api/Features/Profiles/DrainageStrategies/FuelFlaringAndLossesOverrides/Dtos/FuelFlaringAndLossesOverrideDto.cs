using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides.Dtos;

public class FuelFlaringAndLossesOverrideDto : TimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
