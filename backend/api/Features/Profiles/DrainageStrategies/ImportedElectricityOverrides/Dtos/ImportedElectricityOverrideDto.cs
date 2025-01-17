using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;

public class ImportedElectricityOverrideDto : TimeSeriesEnergyDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
