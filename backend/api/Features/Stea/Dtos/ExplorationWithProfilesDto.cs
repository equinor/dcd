using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Explorations.CountryOfficeCosts.Dtos;
using api.Features.Profiles.Explorations.GAndGAdminCostOverrides.Dtos;
using api.Features.Profiles.Explorations.SeismicAcquisitionAndProcessings.Dtos;
using api.Models;

namespace api.Features.Stea.Dtos;

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
