using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.CaseProfiles.Services.TotalFeasibilityAndConceptStudiesOverrides.Dtos;

public class CreateTotalFeasibilityAndConceptStudiesOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
