using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.Cases.CessationOffshoreFacilitiesCostOverrides.Dtos;

public class UpdateCessationOffshoreFacilitiesCostOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
