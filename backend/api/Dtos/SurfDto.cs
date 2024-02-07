using System.ComponentModel.DataAnnotations;

using api.Models;
namespace api.Dtos;

public class SurfDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty!;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public SurfCostProfileDto CostProfile { get; set; } = new SurfCostProfileDto();
    [Required]
    public SurfCostProfileOverrideDto CostProfileOverride { get; set; } = new SurfCostProfileOverrideDto();
    [Required]
    public SurfCessationCostProfileDto CessationCostProfile { get; set; } = new SurfCessationCostProfileDto();
    [Required]
    public double CessationCost { get; set; }
    [Required]
    public Maturity Maturity { get; set; }
    [Required]
    public double InfieldPipelineSystemLength { get; set; }
    [Required]
    public double UmbilicalSystemLength { get; set; }
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public int RiserCount { get; set; }
    [Required]
    public int TemplateCount { get; set; }
    [Required]
    public int ProducerCount { get; set; }
    [Required]
    public int GasInjectorCount { get; set; }
    [Required]
    public int WaterInjectorCount { get; set; }
    [Required]
    public ProductionFlowline ProductionFlowline { get; set; }
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public DateTimeOffset LastChangedDate { get; set; }
    [Required]
    public int CostYear { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    [Required]
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
