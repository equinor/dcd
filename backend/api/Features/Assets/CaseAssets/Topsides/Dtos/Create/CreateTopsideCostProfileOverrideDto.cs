using api.Dtos;

namespace api.Features.Assets.CaseAssets.Topsides.Dtos.Create;

public class CreateTopsideCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
