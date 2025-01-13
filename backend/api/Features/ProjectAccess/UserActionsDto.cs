using System.ComponentModel.DataAnnotations;

namespace api.Features.ProjectAccess;

public class UserActionsDto
{
    [Required] public required bool CanView { get; set; }
    [Required] public required bool CanCreateRevision { get; set; }
    [Required] public required bool CanEditProjectData { get; set; }
    [Required] public required bool CanEditProjectMembers { get; set; }
}
