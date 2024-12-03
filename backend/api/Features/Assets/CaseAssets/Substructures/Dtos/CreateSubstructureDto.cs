using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Dtos;

public class CreateSubstructureDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Source Source { get; set; }
}
