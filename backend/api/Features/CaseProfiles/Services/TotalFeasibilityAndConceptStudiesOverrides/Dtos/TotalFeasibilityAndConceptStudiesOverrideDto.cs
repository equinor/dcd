using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.CaseProfiles.Services.TotalFeasibilityAndConceptStudiesOverrides.Dtos;

public class TotalFeasibilityAndConceptStudiesOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required] public bool Override { get; set; }
}
