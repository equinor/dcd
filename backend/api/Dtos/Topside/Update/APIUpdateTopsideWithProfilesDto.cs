using api.Models;

namespace api.Dtos;

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
