using System.ComponentModel.DataAnnotations;

using api.Models;
namespace api.Dtos;

public class UpdateSurfDto
{
    public string Name { get; set; } = string.Empty!;
    public UpdateSurfCostProfileOverrideDto CostProfileOverride { get; set; } = new UpdateSurfCostProfileOverrideDto();
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
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
    public bool HasChanges { get; set; }
}

public class UpdateSurfCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}


public enum UpdateProductionFlowlineDto
{
    Default = 999
}
