using System.ComponentModel.DataAnnotations;

namespace api.Features.ProjectData.Dtos;

public class RevisionDetailsDto
{
    [Required] public required Guid RevisionId { get; set; }
    [Required] public required string? RevisionName { get; set; }
    [Required] public required DateTime RevisionDate { get; set; }
    [Required] public required bool Arena { get; set; }
    [Required] public required bool Mdqc { get; set; }
}
