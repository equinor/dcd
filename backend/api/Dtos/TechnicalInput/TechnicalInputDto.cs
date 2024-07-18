using api.Models;

namespace api.Dtos;

public class TechnicalInputDto
{
    public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCostsDto { get; set; } = null!;
    public ExplorationOperationalWellCostsDto ExplorationOperationalWellCostsDto { get; set; } = null!;
    public WellDto[]? WellDtos { get; set; }
    public ProjectWithAssetsDto ProjectDto { get; set; } = null!;
    public ExplorationWithProfilesDto? ExplorationDto { get; set; }
    public WellProjectWithProfilesDto? WellProjectDto { get; set; }
}
