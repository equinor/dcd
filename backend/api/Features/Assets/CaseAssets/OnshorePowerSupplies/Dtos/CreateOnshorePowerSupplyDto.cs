using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;

public class CreateOnshorePowerSupplyDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Source Source { get; set; }
}
