using api.Features.Assets.CaseAssets.OnshorePowerSupply.Dtos.Update;
using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;
using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;

public class APIUpdateOnshorePowerSupplyWithProfilesDto : BaseUpdateOnshorePowerSupplyDto
{
    public UpdateOnshorePowerSupplyCostProfileOverrideDto? CostProfileOverride { get; set; }
    public Maturity Maturity { get; set; }
}

public class UpdateOnshorePowerSupplyCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
