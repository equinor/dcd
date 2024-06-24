using api.Models;

namespace api.Dtos;

public class CreateTimeSeriesScheduleDto
{
    public int StartYear { get; set; }
    public int[]? Values { get; set; } = [];
}

public class CreateTimeSeriesCostDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
    public Currency Currency { get; set; }
}

public class CreateTimeSeriesEnergyDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class CreateTimeSeriesMassDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class CreateTimeSeriesVolumeDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}
