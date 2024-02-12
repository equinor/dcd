using System.ComponentModel.DataAnnotations;

using static api.Services.CaseWithAssetsService;

namespace api.Dtos;

public class ProjectWithGeneratedProfilesDto
{
    [Required]
    public ProjectDto ProjectDto { get; set; } = null!;
    [Required]
    public GeneratedProfilesDto GeneratedProfilesDto { get; set; } = null!;
}
