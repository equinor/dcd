using System.ComponentModel.DataAnnotations;

using api.Dtos;

namespace api.Features.Revision.Get;

public class RevisionWithCasesDto : ProjectDto
{
    [Required]
    public DateTimeOffset ModifyTime { get; set; }
    [Required]
    public RevisionDetailsDto RevisionDetails { get; set; } = null!;
    [Required]
    public ICollection<CaseDto> Cases { get; set; } = [];
    [Required]
    public ExplorationOperationalWellCostsDto ExplorationOperationalWellCosts { get; set; } = new();
    [Required]
    public DevelopmentOperationalWellCostsDto DevelopmentOperationalWellCosts { get; set; } = new();
    [Required]
    public ICollection<WellDto> Wells { get; set; } = [];
}
