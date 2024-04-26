using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class PROSPUpdateSubstructureDto : BaseUpdateSubstructureDto
{
    public UpdateSubstructureCostProfileDto? CostProfile { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }


}

public class UpdateSubstructureCostProfileDto : UpdateTimeSeriesCostDto
{
}
