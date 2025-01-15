using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.Transports.TransportCostProfileOverrides.Dtos;

public class TransportCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required] public bool Override { get; set; }
}
