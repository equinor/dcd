using api.Dtos;
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
