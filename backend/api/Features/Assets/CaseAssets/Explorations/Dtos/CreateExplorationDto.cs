using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.Explorations.Dtos;

public class CreateExplorationDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
