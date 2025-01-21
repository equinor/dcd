using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos.BaseClasses;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesScheduleDto : TimeSeriesDto<int>;

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
