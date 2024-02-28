using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class ExplorationOperationalWellCostsDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public double RigUpgrading { get; set; }
    [Required]
    public double ExplorationRigMobDemob { get; set; }
    [Required]
    public double ExplorationProjectDrillingCosts { get; set; }
    [Required]
    public double AppraisalRigMobDemob { get; set; }
    [Required]
    public double AppraisalProjectDrillingCosts { get; set; }
    [Required]
    public bool HasChanges { get; set; }
}
