using System.ComponentModel.DataAnnotations;

using api.Models;
namespace api.Dtos;

public class PROSPUpdateSurfDto : BaseUpdateSurfDto
{
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateSurfCostProfileDto : UpdateTimeSeriesCostDto
{
}

