using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateExplorationDto
{
    public string Name { get; set; } = string.Empty;
    public ExplorationWellCostProfileDto ExplorationWellCostProfile { get; set; } = new ExplorationWellCostProfileDto();
    public AppraisalWellCostProfileDto AppraisalWellCostProfile { get; set; } = new AppraisalWellCostProfileDto();
    public SidetrackCostProfileDto SidetrackCostProfile { get; set; } = new SidetrackCostProfileDto();
    public GAndGAdminCostDto GAndGAdminCost { get; set; } = new GAndGAdminCostDto();
    public SeismicAcquisitionAndProcessingDto SeismicAcquisitionAndProcessing { get; set; } = new SeismicAcquisitionAndProcessingDto();
    public CountryOfficeCostDto CountryOfficeCost { get; set; } = new CountryOfficeCostDto();
    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }
    public List<ExplorationWellDto>? ExplorationWells { get; set; } = [];
}
public class UpdateExplorationWellCostProfileDto : TimeSeriesCostDto
{
}
public class UpdateAppraisalWellCostProfileDto : TimeSeriesCostDto
{
}
public class UpdateSidetrackCostProfileDto : TimeSeriesCostDto
{
}
public class UpdateGAndGAdminCostDto : TimeSeriesCostDto { }
public class UpdateSeismicAcquisitionAndProcessingDto : TimeSeriesCostDto { }
public class UpdateCountryOfficeCostDto : TimeSeriesCostDto { }
