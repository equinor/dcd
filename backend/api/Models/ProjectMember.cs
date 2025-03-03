using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class ProjectMember : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public required Guid UserId { get; set; } // Azure AD user id
    public required ProjectMemberRole Role { get; set; }
    public required bool FromOrgChart { get; set; }

    #region Change tracking

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #endregion
}
