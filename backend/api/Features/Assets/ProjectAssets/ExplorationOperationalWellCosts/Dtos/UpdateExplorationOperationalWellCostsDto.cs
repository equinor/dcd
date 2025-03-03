using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;

public class UpdateExplorationOperationalWellCostsDto
{
    [Required] public required double ExplorationRigUpgrading { get; set; }
    [Required] public required double ExplorationRigMobDemob { get; set; }
    [Required] public required double ExplorationProjectDrillingCosts { get; set; }
    [Required] public required double AppraisalRigMobDemob { get; set; }
    [Required] public required double AppraisalProjectDrillingCosts { get; set; }
}
