using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.Stea.Dtos;

namespace api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;

public class ImportedElectricityOverrideDto : ImportedElectricityDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
