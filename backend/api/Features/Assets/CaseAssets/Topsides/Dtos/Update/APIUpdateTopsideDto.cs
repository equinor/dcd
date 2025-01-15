using api.Models;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Topsides.Dtos.Update;

public class APIUpdateTopsideDto : BaseUpdateTopsideDto
{
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}
