using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos.BaseClasses;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesVolumeDto : TimeSeriesDoubleDto;

public class TimeSeriesVolumeOverrideDto : TimeSeriesVolumeDto
{
    [Required] public bool Override { get; set; }
}

public class CreateTimeSeriesVolumeDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class CreateTimeSeriesVolumeOverrideDto : CreateTimeSeriesVolumeDto
{
    public bool Override { get; set; }
}

public class UpdateTimeSeriesVolumeDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class UpdateTimeSeriesVolumeOverrideDto : UpdateTimeSeriesVolumeDto
{
    public bool Override { get; set; }
}
