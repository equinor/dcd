namespace api.Models;

public class ProjectMember
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // Azure AD user id
    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;
    public ProjectMemberRole Role { get; set; }
}

public enum ProjectMemberRole
{
    Observer,
    Editor,
}

