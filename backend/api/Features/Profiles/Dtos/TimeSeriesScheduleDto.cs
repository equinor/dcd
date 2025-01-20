using api.Features.Profiles.Dtos.BaseClasses;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesScheduleDto : TimeSeriesDto<int>;

public class CreateTimeSeriesScheduleDto
{
    public int StartYear { get; set; }
    public int[]? Values { get; set; } = [];
}

public class UpdateTimeSeriesScheduleDto
{
    public int StartYear { get; set; }
    public int[]? Values { get; set; } = [];
}
