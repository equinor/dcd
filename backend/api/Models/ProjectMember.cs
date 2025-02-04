using api.Models.Interfaces;

namespace api.Models;

public class ProjectMember : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid UserId { get; set; } // Azure AD user id
    public ProjectMemberRole Role { get; set; }
    public bool FromOrgChart { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
}

public enum ProjectMemberRole
{
    Observer,
    Editor,
}
