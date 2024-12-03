using api.Dtos;
using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Dtos.Update;

public class APIUpdateSurfWithProfilesDto : BaseUpdateSurfDto
{
    public UpdateSurfCostProfileOverrideDto? CostProfileOverride { get; set; }
    public Maturity Maturity { get; set; }
}

public class UpdateSurfCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
