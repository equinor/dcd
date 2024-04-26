using api.Models;
namespace api.Dtos;

public class PROSPUpdateTransportDto : BaseUpdateTransportDto
{
    public UpdateTransportCostProfileDto? CostProfile { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateTransportCostProfileDto : UpdateTimeSeriesCostDto
{
}
