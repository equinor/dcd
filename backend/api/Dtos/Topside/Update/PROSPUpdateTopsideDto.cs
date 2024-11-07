namespace api.Dtos;

public class PROSPUpdateTopsideDto : BaseUpdateTopsideDto
{
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateTopsideCostProfileDto : UpdateTimeSeriesCostDto;
