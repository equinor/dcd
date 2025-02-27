using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

public class UpdateOnshorePowerSupplyDto
{
    [Required] public required int CostYear { get; set; }
    [Required] public required Source Source { get; set; }
}
