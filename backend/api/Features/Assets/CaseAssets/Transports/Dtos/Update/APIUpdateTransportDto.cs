using api.Models;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Transports.Dtos.Update;

public class APIUpdateTransportDto : BaseUpdateTransportDto
{
    public Maturity Maturity { get; set; }
}
