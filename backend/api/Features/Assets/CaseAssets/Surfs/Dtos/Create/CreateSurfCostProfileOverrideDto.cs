using api.Dtos;

namespace api.Features.Assets.CaseAssets.Surfs.Dtos.Create;

public class CreateSurfCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
