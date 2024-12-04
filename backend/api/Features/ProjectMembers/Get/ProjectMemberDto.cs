using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.ProjectMembers.Get
{
    public class ProjectMemberDto
    {
        [Required] public required Guid ProjectId { get; set; }
        [Required] public required Guid UserId { get; set; }
        [Required] public required ProjectMemberRole Role { get; set; }
        [Required] public required bool IsPmt { get; set; }
    }
}
