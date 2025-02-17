using System.ComponentModel.DataAnnotations;

namespace api.Features.Profiles.Save;

public class SaveTimeSeriesDto
{
    [Required] public required string ProfileType { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; }
}

public class SaveTimeSeriesOverrideDto
{
    [Required] public required string ProfileType { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; }
    [Required] public required bool Override { get; set; }
}

public class SaveTimeSeriesListDto
{
    [Required] public required List<SaveTimeSeriesDto> TimeSeries { get; set; }
    [Required] public required List<SaveTimeSeriesOverrideDto> OverrideTimeSeries { get; set; }
}
