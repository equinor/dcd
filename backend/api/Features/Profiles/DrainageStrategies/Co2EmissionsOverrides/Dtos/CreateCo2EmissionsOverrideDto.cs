using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides.Dtos;

public class CreateCo2EmissionsOverrideDto : CreateTimeSeriesMassDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
