using api.Models;

namespace api.Dtos;

public class CaseDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool ReferenceCase { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public double FacilitiesAvailability { get; set; }
    public double CapexFactorFeasibilityStudies { get; set; }
    public double CapexFactorFEEDStudies { get; set; }
    public DateTimeOffset DGADate { get; set; }
    public DateTimeOffset DGBDate { get; set; }
    public DateTimeOffset DGCDate { get; set; }
    public DateTimeOffset APXDate { get; set; }
    public DateTimeOffset APZDate { get; set; }
    public DateTimeOffset DG0Date { get; set; }
    public DateTimeOffset DG1Date { get; set; }
    public DateTimeOffset DG2Date { get; set; }
    public DateTimeOffset DG3Date { get; set; }
    public DateTimeOffset DG4Date { get; set; }
    public DateTimeOffset CreateTime { get; set; }
    public DateTimeOffset ModifyTime { get; set; }
    public Guid DrainageStrategyLink { get; set; }
    public Guid WellProjectLink { get; set; }
    public Guid SurfLink { get; set; }
    public Guid SubstructureLink { get; set; }
    public Guid TopsideLink { get; set; }
    public Guid TransportLink { get; set; }
    public Guid ExplorationLink { get; set; }
    public double Capex { get; set; }
    public CapexYear? CapexYear { get; set; }
    public CessationCostDto? CessationCost { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
}

public class CessationCostDto : TimeSeriesCostDto { }
public class OpexCostProfileDto : TimeSeriesCostDto { }
public class StudyCostProfileDto : TimeSeriesCostDto { }

public class CapexYear
{
    public double[]? Values { get; set; }
    public int? StartYear { get; set; }
}
