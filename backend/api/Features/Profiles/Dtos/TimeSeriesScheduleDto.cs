using System.ComponentModel.DataAnnotations;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesScheduleDto
{
    [Required] public Guid Id { get; set; }
    [Required] public int StartYear { get; set; }
    [Required] public int[] Values { get; set; } = [];
}

public class CreateTimeSeriesScheduleDto
{
    [Required] public int StartYear { get; set; }
    [Required] public int[] Values { get; set; } = [];
}

public class UpdateTimeSeriesScheduleDto
{
    [Required] public int StartYear { get; set; }
    [Required] public int[] Values { get; set; } = [];
}
