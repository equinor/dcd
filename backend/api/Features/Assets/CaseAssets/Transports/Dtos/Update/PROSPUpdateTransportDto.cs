using api.Dtos;

namespace api.Features.Assets.CaseAssets.Transports.Dtos.Update;

public class PROSPUpdateTransportDto : BaseUpdateTransportDto
{
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateTransportCostProfileDto : UpdateTimeSeriesCostDto;
