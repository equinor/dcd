using System.ComponentModel.DataAnnotations;

namespace api.Features.Profiles.Dtos;

public class CreateTimeSeriesDto
{
    [Required] public required string ProfileType { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; }
}

public class CreateTimeSeriesOverrideDto
{
    [Required] public required string ProfileType { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; }
    [Required] public required bool Override { get; set; }
}

public class UpdateTimeSeriesDto
{
    [Required] public required string ProfileType { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; }
}

public class UpdateTimeSeriesOverrideDto
{
    [Required] public required string ProfileType { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; }
    [Required] public required bool Override { get; set; }
}
