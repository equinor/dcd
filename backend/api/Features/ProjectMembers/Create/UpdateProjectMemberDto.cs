using api.Models;

namespace api.Dtos;

public class UpdateProjectMemberDto
{
    public ProjectMemberRole Role { get; set; }
    public Guid UserId { get; set; }
}
