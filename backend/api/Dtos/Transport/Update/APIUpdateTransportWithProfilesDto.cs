using api.Models;
namespace api.Dtos;

public class APIUpdateTransportWithProfilesDto : BaseUpdateTransportDto
{
    public UpdateTransportCostProfileOverrideDto? CostProfileOverride { get; set; }
    public Maturity Maturity { get; set; }
}

public class UpdateTransportCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
