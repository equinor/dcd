using api.Models;

namespace api.Dtos;

public class PROSPUpdateTopsideDto : BaseUpdateTopsideDto
{
    public UpdateTopsideCostProfileDto? CostProfile { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
}


public class UpdateTopsideCostProfileDto : UpdateTimeSeriesCostDto
{
}
