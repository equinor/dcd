using api.Models;

namespace api.Dtos;

public class TechnicalInputDto
{
    public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCostsDto { get; set; } = null!;
    public ExplorationOperationalWellCostsDto ExplorationOperationalWellCostsDto { get; set; } = null!;
    public WellDto[]? WellDtos { get; set; }
    public ProjectDto ProjectDto { get; set; } = null!;
    public ExplorationDto? ExplorationDto { get; set; }
    public WellProjectDto? WellProjectDto { get; set; }
    public Guid? CaseId { get; set; }
}
