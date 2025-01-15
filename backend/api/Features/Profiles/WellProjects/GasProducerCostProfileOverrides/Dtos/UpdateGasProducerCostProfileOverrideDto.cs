using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides.Dtos;

public class UpdateGasProducerCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
