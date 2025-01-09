namespace api.AppInfrastructure.Authorization;

public class CurrentUser
{
    public required string Username { get; set; }
    public required Guid UserId { get; set; }
    public required HashSet<ApplicationRole> ApplicationRoles { get; set; }
}
