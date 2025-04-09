using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.TableRanges;

public class UpdateTableRangesDto
{
    [Required] public required int[] Co2EmissionsYears { get; set; }
    [Required] public required int[] DrillingScheduleYears { get; set; }
    [Required] public required int[] CaseCostYears { get; set; }
    [Required] public required int[] ProductionProfilesYears { get; set; }
} 
