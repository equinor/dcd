using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Project.ProjectMember.Create
{
    public class ProjectMemberDto
    {
        [Required] public Guid ProjectId { get; set; }
        [Required] public Guid UserId { get; set; }
        [Required] public ProjectMemberRole Role { get; set; }
    }
}
