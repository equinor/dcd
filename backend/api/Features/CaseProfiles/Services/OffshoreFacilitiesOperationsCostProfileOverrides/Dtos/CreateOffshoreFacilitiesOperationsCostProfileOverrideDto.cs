using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;

public class CreateOffshoreFacilitiesOperationsCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
