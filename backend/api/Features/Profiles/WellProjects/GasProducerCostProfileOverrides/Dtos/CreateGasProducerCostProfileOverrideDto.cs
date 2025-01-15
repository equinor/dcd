using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides.Dtos;

public class CreateGasProducerCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
