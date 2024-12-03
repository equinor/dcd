using System.ComponentModel.DataAnnotations;

using api.Dtos;
using api.Models;

namespace api.Features.Assets.CaseAssets.Explorations.Dtos;

public class ExplorationWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public ExplorationWellCostProfileDto ExplorationWellCostProfile { get; set; } = new();
    [Required]
    public AppraisalWellCostProfileDto AppraisalWellCostProfile { get; set; } = new();
    [Required]
    public SidetrackCostProfileDto SidetrackCostProfile { get; set; } = new();
    [Required]
    public GAndGAdminCostDto GAndGAdminCost { get; set; } = new();
    [Required]
    public GAndGAdminCostOverrideDto GAndGAdminCostOverride { get; set; } = new();
    [Required]
    public SeismicAcquisitionAndProcessingDto SeismicAcquisitionAndProcessing { get; set; } = new();
    [Required]
    public CountryOfficeCostDto CountryOfficeCost { get; set; } = new();
    [Required]
    public double RigMobDemob { get; set; }
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public List<ExplorationWellDto>? ExplorationWells { get; set; } = [];
}

public class ExplorationWellCostProfileDto : TimeSeriesCostDto;

public class AppraisalWellCostProfileDto : TimeSeriesCostDto;

public class SidetrackCostProfileDto : TimeSeriesCostDto;

public class GAndGAdminCostDto : TimeSeriesCostDto;

public class GAndGAdminCostOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class SeismicAcquisitionAndProcessingDto : TimeSeriesCostDto;

public class CountryOfficeCostDto : TimeSeriesCostDto;
