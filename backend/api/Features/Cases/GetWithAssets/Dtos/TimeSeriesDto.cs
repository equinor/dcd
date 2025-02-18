using System.ComponentModel.DataAnnotations;

namespace api.Features.Cases.GetWithAssets.Dtos;

public class TimeSeriesDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public DateTime UpdatedUtc { get; set; }
}

public class TimeSeriesOverrideDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public bool Override { get; set; }
    [Required] public DateTime UpdatedUtc { get; set; }
}
