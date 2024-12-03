using api.Dtos;

namespace api.Features.Assets.CaseAssets.Substructures.Dtos.Update;

public class PROSPUpdateSubstructureDto : BaseUpdateSubstructureDto
{
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateSubstructureCostProfileDto : UpdateTimeSeriesCostDto;
