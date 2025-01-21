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
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}

public class CreateTimeSeriesVolumeOverrideDto : CreateTimeSeriesVolumeDto
{
    [Required] public bool Override { get; set; }
}

public class UpdateTimeSeriesVolumeDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}

public class UpdateTimeSeriesVolumeOverrideDto : UpdateTimeSeriesVolumeDto
{
    [Required] public bool Override { get; set; }
}
