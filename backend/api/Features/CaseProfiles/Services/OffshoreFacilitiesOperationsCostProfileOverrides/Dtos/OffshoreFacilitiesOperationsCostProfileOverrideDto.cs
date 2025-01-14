using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;

public class OffshoreFacilitiesOperationsCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required] public bool Override { get; set; }
}
