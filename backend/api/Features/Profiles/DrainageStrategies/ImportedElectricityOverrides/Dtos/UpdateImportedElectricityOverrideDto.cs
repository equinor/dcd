using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Update;

namespace api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;

public class UpdateImportedElectricityOverrideDto : UpdateTimeSeriesEnergyDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
