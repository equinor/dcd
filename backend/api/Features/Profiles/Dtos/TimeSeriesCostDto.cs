using System.ComponentModel.DataAnnotations;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesCostDto
{
    [Required] public Guid Id { get; set; }
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public DateTime UpdatedUtc { get; set; }
}

public class TimeSeriesCostOverrideDto
{
    [Required] public Guid Id { get; set; }
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public bool Override { get; set; }
    [Required] public DateTime UpdatedUtc { get; set; }
}

public class UpdateTimeSeriesCostDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}
