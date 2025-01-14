using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;

public class UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
