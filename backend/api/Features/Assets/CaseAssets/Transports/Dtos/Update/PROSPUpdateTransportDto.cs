using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Assets.CaseAssets.Transports.Dtos.Update;

public class PROSPUpdateTransportDto : BaseUpdateTransportDto
{
    public DateTime? ProspVersion { get; set; }
}

public class UpdateTransportCostProfileDto : UpdateTimeSeriesCostDto;
