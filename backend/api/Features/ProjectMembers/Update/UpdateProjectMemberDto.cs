using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.ProjectMembers.Update;

public class UpdateProjectMemberDto
{
    [Required] public required ProjectMemberRole Role { get; set; }
    [Required] public required Guid AzureAdUserId { get; set; }
}
