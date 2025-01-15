using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.CaseProfiles.Services.CessationOffshoreFacilitiesCostOverrides.Dtos;

public class CessationOffshoreFacilitiesCostOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required] public bool Override { get; set; }
}
