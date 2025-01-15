using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides.Dtos;

public class Co2EmissionsOverrideDto : TimeSeriesMassDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
