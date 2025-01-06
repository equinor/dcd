
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;

public class PROSPUpdateOnshorePowerSupplyDto : BaseUpdateOnshorePowerSupplyDto
{
    public DateTime? ProspVersion { get; set; }
}

public class UpdateOnshorePowerSupplyCostProfileDto : UpdateTimeSeriesCostDto;
