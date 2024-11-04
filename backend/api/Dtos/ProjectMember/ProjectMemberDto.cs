using System.ComponentModel.DataAnnotations;

using api.Models;

public class ProjectMemberDto
{
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public ProjectMemberRole Role { get; set; }
}
