using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class PROSPUpdateSubstructureDto : BaseUpdateSubstructureDto
{
    public DateTimeOffset? ProspVersion { get; set; }
}

public class UpdateSubstructureCostProfileDto : UpdateTimeSeriesCostDto
{
}
