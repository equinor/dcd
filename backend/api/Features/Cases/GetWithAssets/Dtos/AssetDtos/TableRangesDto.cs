using System.ComponentModel.DataAnnotations;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class TableRangesDto
{
    [Required] public required int[] Co2EmissionsYears { get; set; }
    [Required] public required int[] DrillingScheduleYears { get; set; }
    [Required] public required int[] CaseCostYears { get; set; }
    [Required] public required int[] ProductionProfilesYears { get; set; }
}
