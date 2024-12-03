using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;
using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Dtos.Update;

public class APIUpdateSubstructureWithProfilesDto : BaseUpdateSubstructureDto
{
    public UpdateSubstructureCostProfileOverrideDto? CostProfileOverride { get; set; }
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}


public class UpdateSubstructureCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
