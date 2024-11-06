using api.Models;

namespace api.Dtos;

public class CreateProjectMemberDto
{
    public ProjectMemberRole Role { get; set; }
    public Guid UserId { get; set; }
}
