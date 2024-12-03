using api.Dtos;

namespace api.Features.Assets.CaseAssets.Surfs.Dtos.Update;

public class PROSPUpdateSurfDto : BaseUpdateSurfDto
{
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateSurfCostProfileDto : UpdateTimeSeriesCostDto;
