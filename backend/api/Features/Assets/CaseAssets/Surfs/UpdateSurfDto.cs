using api.Models;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Surfs;

public class UpdateSurfDto
{
    public double CessationCost { get; set; }
    public double InfieldPipelineSystemLength { get; set; }
    public double UmbilicalSystemLength { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public int RiserCount { get; set; }
    public int TemplateCount { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public ProductionFlowline ProductionFlowline { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Maturity Maturity { get; set; }
}
