using api.Models;

namespace api.Dtos;

public class UpdateTechnicalInputDto
{
    public UpdateDevelopmentOperationalWellCostsDto DevelopmentOperationalWellCostsDto { get; set; } = null!;
    public UpdateExplorationOperationalWellCostsDto ExplorationOperationalWellCostsDto { get; set; } = null!;
    public UpdateWellDto[]? UpdateWellDtos { get; set; }
    public CreateWellDto[]? CreateWellDtos { get; set; }
    public DeleteWellDto[]? DeleteWellDtos { get; set; }
    public UpdateProjectDto ProjectDto { get; set; } = null!;
    public UpdateExplorationWithProfilesDto? ExplorationDto { get; set; }
    public UpdateWellProjectWithProfilesDto? WellProjectDto { get; set; }
}
