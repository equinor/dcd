using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.Cases.CessationWellsCostOverrides.Dtos;

public class UpdateCessationWellsCostOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
