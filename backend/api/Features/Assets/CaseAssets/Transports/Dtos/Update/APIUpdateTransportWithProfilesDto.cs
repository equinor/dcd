using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;
using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Dtos.Update;

public class APIUpdateTransportWithProfilesDto : BaseUpdateTransportDto
{
    public UpdateTransportCostProfileOverrideDto? CostProfileOverride { get; set; }
    public Maturity Maturity { get; set; }
}

public class UpdateTransportCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
