using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.Profiles.WellProjects.OilProducerCostProfileOverrides.Dtos;

public class CreateOilProducerCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
