using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Revisions.Create;

public class CreateRevisionDto
{
    [Required]
    public string Name { get; set; } = null!;
    public InternalProjectPhase InternalProjectPhase { get; set; }
    public ProjectClassification Classification { get; set; }
    public bool Arena { get; set; }
    public bool Mdqc { get; set; }
}
