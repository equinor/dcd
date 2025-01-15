using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.Profiles.WellProjects.GasInjectorCostProfileOverrides.Dtos;

public class CreateGasInjectorCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
