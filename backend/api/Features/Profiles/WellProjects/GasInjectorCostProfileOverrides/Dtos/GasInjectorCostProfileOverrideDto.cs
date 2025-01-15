using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.WellProjects.GasInjectorCostProfileOverrides.Dtos;

public class GasInjectorCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required] public bool Override { get; set; }
}
