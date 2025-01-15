using System.ComponentModel.DataAnnotations;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class DevelopmentOperationalWellCostsOverviewDto
{
    [Required] public required Guid DevelopmentOperationalWellCostsId { get; set; }
    [Required] public required double RigUpgrading { get; set; }
    [Required] public required double RigMobDemob { get; set; }
    [Required] public required double AnnualWellInterventionCostPerWell { get; set; }
    [Required] public required double PluggingAndAbandonment { get; set; }
}
