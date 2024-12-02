namespace api.Features.ProjectAccess;

public class AccessRightsDto
{
    public required bool CanEdit { get; set; }
    public required bool CanView { get; set; }
    public required bool IsAdmin { get; set; }
}
