using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.ProjectData.Dtos;

public class RevisionDetailsDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required Guid RevisionId { get; set; }
    [Required] public required string? RevisionName { get; set; }
    [Required] public required DateTimeOffset RevisionDate { get; set; }
    [Required] public required bool Arena { get; set; }
    [Required] public required bool Mdqc { get; set; }
}
