using api.Models;

namespace api.Features.Project.ProjectMember.Create;

public class CreateProjectMemberDto
{
    public ProjectMemberRole Role { get; set; }
    public Guid UserId { get; set; }
}
