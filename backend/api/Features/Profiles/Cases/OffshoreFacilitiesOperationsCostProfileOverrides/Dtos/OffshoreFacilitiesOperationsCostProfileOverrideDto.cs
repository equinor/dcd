using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;

public class OffshoreFacilitiesOperationsCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required] public bool Override { get; set; }
}
