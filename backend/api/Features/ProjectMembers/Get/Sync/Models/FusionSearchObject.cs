namespace api.Features.ProjectMembers.Get.Sync.Models;

public class FusionSearchObject
{
    public string Filter { get; set; } = string.Empty;
    public string[] OrderBy { get; set; } = [];
    public int? Top { get; set; }
    public int? Skip { get; set; }
    public bool IncludeTotalResultCount { get; set; } = true;
    public bool MatchAll { get; set; } = true;
    public bool FullQueryMode { get; set; } = true;
}
