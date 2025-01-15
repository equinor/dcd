using System.ComponentModel.DataAnnotations;

using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;

public class ImportedElectricityOverrideDto : ImportedElectricityDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
