using System.ComponentModel.DataAnnotations;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesCostDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public DateTime UpdatedUtc { get; set; }
}

public class TimeSeriesCostOverrideDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public bool Override { get; set; }
    [Required] public DateTime UpdatedUtc { get; set; }
}
