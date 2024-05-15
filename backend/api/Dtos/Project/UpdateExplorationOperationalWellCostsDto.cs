using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class UpdateExplorationOperationalWellCostsDto
{
    public double ExplorationRigUpgrading { get; set; }
    public double ExplorationRigMobDemob { get; set; }
    public double ExplorationProjectDrillingCosts { get; set; }
    public double AppraisalRigMobDemob { get; set; }
    public double AppraisalProjectDrillingCosts { get; set; }
}
