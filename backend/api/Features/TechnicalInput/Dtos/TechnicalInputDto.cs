using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.ProjectData.Dtos;
using api.Features.Wells.Get;

namespace api.Features.TechnicalInput.Dtos;

public class TechnicalInputDto
{
    public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCostsDto { get; set; } = null!;
    public ExplorationOperationalWellCostsDto ExplorationOperationalWellCostsDto { get; set; } = null!;
    public WellDto[]? WellDtos { get; set; }
    public ProjectDataDto ProjectData { get; set; } = null!;
    public ExplorationWithProfilesDto? ExplorationDto { get; set; }
    public WellProjectWithProfilesDto? WellProjectDto { get; set; }
}
