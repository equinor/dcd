using api.Models;

namespace api.Features.CaseProfiles.Dtos.TimeSeries.Update;

public class UpdateTimeSeriesScheduleDto
{
    public int StartYear { get; set; }
    public int[]? Values { get; set; } = [];
}

public class UpdateTimeSeriesCostDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
    public Currency Currency { get; set; }
}

public class UpdateTimeSeriesEnergyDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class UpdateTimeSeriesMassDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class UpdateTimeSeriesVolumeDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}
