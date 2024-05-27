using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class ExplorationWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public ExplorationWellCostProfileDto ExplorationWellCostProfile { get; set; } = new ExplorationWellCostProfileDto();
    [Required]
    public AppraisalWellCostProfileDto AppraisalWellCostProfile { get; set; } = new AppraisalWellCostProfileDto();
    [Required]
    public SidetrackCostProfileDto SidetrackCostProfile { get; set; } = new SidetrackCostProfileDto();
    [Required]
    public GAndGAdminCostDto GAndGAdminCost { get; set; } = new GAndGAdminCostDto();
    [Required]
    public SeismicAcquisitionAndProcessingDto SeismicAcquisitionAndProcessing { get; set; } = new SeismicAcquisitionAndProcessingDto();
    [Required]
    public CountryOfficeCostDto CountryOfficeCost { get; set; } = new CountryOfficeCostDto();
    [Required]
    public double RigMobDemob { get; set; }
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public List<ExplorationWellDto>? ExplorationWells { get; set; } = [];
}
public class ExplorationWellCostProfileDto : TimeSeriesCostDto
{
}
public class AppraisalWellCostProfileDto : TimeSeriesCostDto
{
}
public class SidetrackCostProfileDto : TimeSeriesCostDto
{
}
public class GAndGAdminCostDto : TimeSeriesCostDto { }
public class SeismicAcquisitionAndProcessingDto : TimeSeriesCostDto { }
public class CountryOfficeCostDto : TimeSeriesCostDto { }
