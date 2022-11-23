using api.Models;

namespace api.Dtos;

public class TechnicalInputDto
{
    public CaseDto CaseDto { get; set; } = null!;
    public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCostsDto { get; set; } = null!;
    public ExplorationOperationalWellCostsDto ExplorationOperationalWellCostsDto { get; set; } = null!;
    public WellDto[] WellDtos { get; set; } = null!;
    public ProjectDto ProjectDto { get; set; } = null!;
}
