using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class ExplorationDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public double RigMobDemob { get; set; }
    [Required]
    public Currency Currency { get; set; }
}

public class ExplorationWellCostProfileDto : TimeSeriesCostDto;

public class AppraisalWellCostProfileDto : TimeSeriesCostDto;

public class SidetrackCostProfileDto : TimeSeriesCostDto;

public class GAndGAdminCostDto : TimeSeriesCostDto;
