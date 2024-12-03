using api.Dtos;

namespace api.Features.Assets.CaseAssets.Transports.Dtos.Create;

public class CreateTransportCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
