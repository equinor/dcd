using api.Models;
namespace api.Dtos;

public class APIUpdateTransportDto : BaseUpdateTransportDto
{
    public UpdateTransportCostProfileOverrideDto? CostProfileOverride { get; set; }
    public Maturity Maturity { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateTransportCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
