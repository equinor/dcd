using api.Models;
namespace api.Dtos;

public class SurfDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty!;
    public Guid ProjectId { get; set; }
    public SurfCostProfileDto? CostProfile { get; set; }
    public SurfCostProfileOverrideDto? CostProfileOverride { get; set; }
    public SurfCessationCostProfileDto? CessationCostProfile { get; set; }
    public double CessationCost { get; set; }
    public Maturity Maturity { get; set; }
    public double InfieldPipelineSystemLength { get; set; }
    public double UmbilicalSystemLength { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public int RiserCount { get; set; }
    public int TemplateCount { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public ProductionFlowline ProductionFlowline { get; set; }
    public Currency Currency { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
    public bool HasChanges { get; set; }
}

public class SurfCostProfileDto : TimeSeriesCostDto
{

}

public class SurfCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class SurfCessationCostProfileDto : TimeSeriesCostDto
{

}

public enum ProductionFlowlineDto
{
    Default = 999
}
