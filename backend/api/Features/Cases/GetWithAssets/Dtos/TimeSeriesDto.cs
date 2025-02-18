using System.ComponentModel.DataAnnotations;

namespace api.Features.Cases.GetWithAssets.Dtos;

public class TimeSeriesDto
{
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; } = [];
    [Required] public required DateTime UpdatedUtc { get; set; }
}

public class TimeSeriesOverrideDto
{
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; } = [];
    [Required] public required bool Override { get; set; }
    [Required] public required DateTime UpdatedUtc { get; set; }
}
