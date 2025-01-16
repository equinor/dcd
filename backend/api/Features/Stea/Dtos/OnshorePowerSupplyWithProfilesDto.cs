using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Stea.Dtos;

public class OnshorePowerSupplyCostProfileDto : TimeSeriesCostDto;

public class OnshorePowerSupplyCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
