using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Assets.CaseAssets.Surfs.Dtos.Update;

public class PROSPUpdateSurfDto : BaseUpdateSurfDto
{
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateSurfCostProfileDto : UpdateTimeSeriesCostDto;
