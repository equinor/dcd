using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.ProjectData.Dtos;

namespace api.Features.TechnicalInput.Dtos;

public class TechnicalInputDto
{
    public ProjectDataDto ProjectData { get; set; } = null!;
    public ExplorationWithProfilesDto? ExplorationDto { get; set; }
    public WellProjectWithProfilesDto? WellProjectDto { get; set; }
}
