using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Dtos.Update;

public class APIUpdateSubstructureDto : BaseUpdateSubstructureDto
{
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}
