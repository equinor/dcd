using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.CaseProfiles.Services.TotalFeasibilityAndConceptStudiesOverrides.Dtos;

public class UpdateTotalFeasibilityAndConceptStudiesOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
