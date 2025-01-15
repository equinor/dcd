using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.Profiles.Cases.WellInterventionCostProfileOverrides.Dtos;

public class CreateWellInterventionCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
