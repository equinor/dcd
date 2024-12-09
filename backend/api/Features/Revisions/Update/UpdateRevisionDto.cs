using System.ComponentModel.DataAnnotations;

namespace api.Features.Revisions.Update;

public class UpdateRevisionDto
{
    [Required] public required string Name { get; set; }
    [Required] public required bool Arena { get; set; }
    [Required] public required bool Mdqc { get; set; }
}
