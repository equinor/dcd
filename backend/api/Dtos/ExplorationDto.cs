using api.Models;

namespace api.Dtos;

public class ExplorationDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ExplorationCostProfileDto? CostProfile { get; set; }
    public GAndGAdminCostDto? GAndGAdminCost { get; set; }
    public SeismicAcquisitionAndProcessingDto? SeismicAcquisitionAndProcessing { get; set; }
    public CountryOfficeCostDto? CountryOfficeCost { get; set; }
    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }
    public List<ExplorationWellDto>? ExplorationWells { get; set; }
}

public class ExplorationCostProfileDto : TimeSeriesCostDto
{
    public bool Override { get; set; }
}
public class GAndGAdminCostDto : TimeSeriesCostDto { }
public class SeismicAcquisitionAndProcessingDto : TimeSeriesCostDto { }
public class CountryOfficeCostDto : TimeSeriesCostDto { }