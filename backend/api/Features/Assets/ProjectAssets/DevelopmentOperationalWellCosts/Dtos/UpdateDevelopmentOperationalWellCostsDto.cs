using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;

public class UpdateDevelopmentOperationalWellCostsDto
{
    [Required] public required double RigUpgrading { get; set; }
    [Required] public required double RigMobDemob { get; set; }
    [Required] public required double AnnualWellInterventionCostPerWell { get; set; }
    [Required] public required double PluggingAndAbandonment { get; set; }
}
