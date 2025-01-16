using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.Surfs.SurfCostProfileOverrides.Dtos;

public class UpdateSurfCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
