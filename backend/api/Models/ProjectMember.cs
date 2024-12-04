using api.Models.Interfaces;

namespace api.Models;

public class ProjectMember : IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public Guid UserId { get; set; } // Azure AD user id
    public ProjectMemberRole Role { get; set; }
    public bool FromOrgChart { get; set; }
}

public enum ProjectMemberRole
{
    Observer,
    Editor,
}
