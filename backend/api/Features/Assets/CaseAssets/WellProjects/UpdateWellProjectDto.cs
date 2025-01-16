using api.Models;

namespace api.Features.Assets.CaseAssets.WellProjects;

public class UpdateWellProjectDto
{
    public string Name { get; set; } = string.Empty;
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
}
