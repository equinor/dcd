namespace api.Features.ProjectMembers.Get.Sync.Models;

public class FusionPersonResponseV1
{
    public required List<FusionPersonResultV1> Results { get; set; }
}

public class FusionPersonResultV1
{
    public required FusionPersonV1 Document { get; set; }
}

public class FusionPersonV1
{
    public Guid AzureUniqueId { get; set; }
    public string? Mail { get; set; }
    public string? Name { get; set; }
    public List<ApiPosition>? Positions { get; set; }
}

public class ApiProject
{
    public required string Id { get; set; }
    public bool? IsProjectManagementTeam { get; set; }
}

public class ApiPosition
{
    public ApiProject? Project { get; set; }
    public DateTime? AppliesFrom { get; set; }
    public DateTime? AppliesTo { get; set; }
}

public class FusionPersonDto
{
    public required string Name { get; set; }
    public required string Mail { get; set; }
    public required Guid AzureUniqueId { get; set; }
}
