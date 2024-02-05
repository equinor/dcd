using static api.Services.CaseWithAssetsService;

namespace api.Dtos;

public class ProjectWithGeneratedProfilesDto
{
    public ProjectDto ProjectDto { get; set; } = null!;
    public GeneratedProfilesDto GeneratedProfilesDto { get; set; } = null!;
}
