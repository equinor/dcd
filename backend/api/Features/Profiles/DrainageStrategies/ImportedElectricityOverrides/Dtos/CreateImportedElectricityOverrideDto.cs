using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Dtos.TimeSeries.Create;

namespace api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;

public class CreateImportedElectricityOverrideDto : CreateTimeSeriesEnergyDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
