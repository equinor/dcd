using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.Explorations.GAndGAdminCostOverrides.Dtos;

public class GAndGAdminCostOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
