using System.ComponentModel.DataAnnotations;

using api.Models;
namespace api.Dtos;

public class PROSPUpdateSurfDto : BaseUpdateSurfDto
{
    public UpdateSurfCostProfileDto? CostProfile { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateSurfCostProfileDto : UpdateTimeSeriesCostDto
{
}

