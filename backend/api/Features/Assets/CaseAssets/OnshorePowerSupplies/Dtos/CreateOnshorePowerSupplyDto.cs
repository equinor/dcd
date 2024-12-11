using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;

public class CreateOnshorePowerSupplyDto
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public Source Source { get; set; }
}
