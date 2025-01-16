using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides.Dtos;

public class NetSalesGasOverrideDto : TimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
