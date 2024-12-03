using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Assets.CaseAssets.Topsides.Dtos.Create;

public class CreateTopsideDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Source Source { get; set; }
}
