using api.Models.Interfaces;

namespace api.Models;

public class ProjectMember : IHasProjectId
{
    public Guid AzureUniqueId { get; set; }
    public Guid ProjectId { get; set; }
    public ProjectRole Role { get; set; }
    public string? Name { get; set; }
}

public enum ProjectRole
{
    Contributor,
    Reader,
}
