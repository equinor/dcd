using System.ComponentModel.DataAnnotations;

using api.Dtos;
using api.Features.Wells.Get;

namespace api.Features.Revisions.Get;

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
