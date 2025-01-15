using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides.Dtos;

public class UpdateCo2EmissionsOverrideDto : UpdateTimeSeriesMassDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
