using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Project.Revision;

public class RevisionWithCasesDto : ProjectDto
{
    [Required]
    public DateTimeOffset ModifyTime { get; set; }
    [Required]
    public RevisionDetailsDto RevisionDetails { get; set; } = null!;
    [Required]
    public ICollection<CaseDto> Cases { get; set; } = [];
    [Required]
    public ExplorationOperationalWellCostsDto ExplorationOperationalWellCosts { get; set; } = new ExplorationOperationalWellCostsDto();
    [Required]
    public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCosts { get; set; } = new DevelopmentOperationalWellCostsDto();
}
