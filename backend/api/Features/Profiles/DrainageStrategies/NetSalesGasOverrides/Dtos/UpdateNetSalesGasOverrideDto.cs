using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides.Dtos;

public class UpdateNetSalesGasOverrideDto : UpdateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
