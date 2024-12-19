using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Revisions.Create;

public class CreateRevisionDto
{
    [Required] public required string Name { get; set; }
    [Required] public InternalProjectPhase InternalProjectPhase { get; set; }
    [Required] public ProjectClassification Classification { get; set; }
    [Required] public required bool Arena { get; set; }
    [Required] public required bool Mdqc { get; set; }
}
