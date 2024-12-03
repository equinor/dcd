using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;
using api.Models;

namespace api.Features.Assets.CaseAssets.Topsides.Dtos.Update;

public class APIUpdateTopsideWithProfilesDto : BaseUpdateTopsideDto
{
    public UpdateTopsideCostProfileOverrideDto? CostProfileOverride { get; set; }
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}


public class UpdateTopsideCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
