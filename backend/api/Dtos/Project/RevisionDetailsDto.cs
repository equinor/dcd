using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class RevisionDetailsDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid OriginalProjectId { get; set; }
    [Required]
    public string? RevisionName { get; set; }
    [Required]
    public DateTimeOffset RevisionDate { get; set; }
    [Required]
    public bool Arena { get; set; }
    [Required]
    public bool Mdqc { get; set; }
}