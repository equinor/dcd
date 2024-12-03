using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.WellProjects.Dtos;

public class CreateWellProjectDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
