using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class RevisionDetailsDto
{
    [Required]
    public Guid Id { get; set; }
    public Guid OriginalProjectId { get; set; }
    public string? RevisionName { get; set; }
    public DateTimeOffset RevisionDate { get; set; }
    public bool Arena { get; set; }
    public bool Mdqc { get; set; }
}