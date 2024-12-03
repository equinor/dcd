using api.Dtos;

namespace api.Features.Assets.CaseAssets.Substructures.Dtos.Create;

public class CreateSubstructureCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
