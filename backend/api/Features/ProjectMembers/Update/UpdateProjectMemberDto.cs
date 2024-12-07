using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.ProjectMembers.Update;

public class UpdateProjectMemberDto
{
    [Required] public required ProjectMemberRole Role { get; set; }
    [Required] public required Guid UserId { get; set; }
}
