using Microsoft.Graph;

using File = Microsoft.Graph.File;

namespace api.Dtos;

public class DriveItemDto
{
    public string? Name { get; set; }
    public string? Id { get; set; }
    public string? WebUrl { get; set; }
    public DateTimeOffset? CreatedDateTime { get; set; }
    public  Stream? Content { get; set; }
    public Int64? Size { get; set; }
    public SharepointIds? SharepointIds { get; set; }
    public IdentitySet? CreatedBy { get; set; }
    public IdentitySet? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDateTime { get; set; }
}

