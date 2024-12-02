using api.Models;

namespace api.Features.ProjectMembers.Update;

public class UpdateProjectMemberDto
{
    public ProjectMemberRole Role { get; set; }
    public Guid UserId { get; set; }
}
