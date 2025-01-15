using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;

public class UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
