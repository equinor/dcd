namespace api.Features.Projects.Exists;

public class ProjectExistsDto
{
    public required bool ProjectExists { get; set; }
    public required bool CanCreateProject { get; set; }
}
