using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Surfs.Dtos.Update;

public class APIUpdateSurfDto : BaseUpdateSurfDto
{
    public Maturity Maturity { get; set; }
}

