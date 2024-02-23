using api.Models;

namespace api.Dtos;

public class UpdateTechnicalInputDto
{
    public UpdateDevelopmentOperationalWellCostsDto DevelopmentOperationalWellCostsDto { get; set; } = null!;
    public UpdateExplorationOperationalWellCostsDto ExplorationOperationalWellCostsDto { get; set; } = null!;
    public UpdateWellDto[]? UpdateWellDtos { get; set; }
    public CreateWellDto[]? CreateWellDtos { get; set; }
    public UpdateProjectDto ProjectDto { get; set; } = null!;
    public UpdateExplorationDto? ExplorationDto { get; set; }
    public UpdateWellProjectDto? WellProjectDto { get; set; }
}
