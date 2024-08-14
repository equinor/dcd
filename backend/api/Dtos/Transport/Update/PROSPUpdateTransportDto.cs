using api.Models;
namespace api.Dtos;

public class PROSPUpdateTransportDto : BaseUpdateTransportDto
{
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateTransportCostProfileDto : UpdateTimeSeriesCostDto
{
}
