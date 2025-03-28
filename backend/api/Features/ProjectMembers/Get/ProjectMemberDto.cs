using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.ProjectMembers.Get;

public class ProjectMemberDto
{
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required Guid AzureAdUserId { get; set; }
    [Required] public required ProjectMemberRole Role { get; set; }
    [Required] public required bool IsPmt { get; set; }
}
