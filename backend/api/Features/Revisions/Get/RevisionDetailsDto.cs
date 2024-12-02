using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Revisions.Get;

public class RevisionDetailsDto
{
    [Required] public Guid Id { get; set; }
    [Required] public Guid OriginalProjectId { get; set; }
    [Required] public Guid RevisionId { get; set; }
    [Required] public string? RevisionName { get; set; }
    [Required] public DateTimeOffset RevisionDate { get; set; }
    [Required] public bool Arena { get; set; }
    [Required] public bool Mdqc { get; set; }
    [Required] public ProjectClassification Classification { get; set; }
}
