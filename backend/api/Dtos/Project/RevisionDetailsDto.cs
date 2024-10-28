using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class RevisionDetailsDto
{
    // Hvor mange av disse skal være required?
    // skal er alt som ikke er optional required?
    [Required]
    public Guid Id { get; set; }
    public Guid OriginalProjectId { get; set; }

    public virtual Project Revision { get; set; } = null!;

    public string? RevisionName { get; set; }
    public DateTimeOffset RevisionDate { get; set; }
    public bool Arena { get; set; }
    public bool Mdqc { get; set; }
}