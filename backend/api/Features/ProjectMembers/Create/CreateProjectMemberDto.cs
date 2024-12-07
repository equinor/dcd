using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.ProjectMembers.Create;

public class CreateProjectMemberDto
{
    [Required] public required ProjectMemberRole Role { get; set; }
    [Required] public required Guid UserId { get; set; }
}
