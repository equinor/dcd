using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class UpdateDevelopmentOperationalWellCostsDto
{
    public double RigUpgrading { get; set; }
    public double RigMobDemob { get; set; }
    public double AnnualWellInterventionCostPerWell { get; set; }
    public double PluggingAndAbandonment { get; set; }
}
