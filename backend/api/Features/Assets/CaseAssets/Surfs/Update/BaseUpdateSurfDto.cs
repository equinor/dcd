using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Update;

public abstract class BaseUpdateSurfDto
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
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
}

public class ProspUpdateSurfDto : BaseUpdateSurfDto
{
    public DateTime? ProspVersion { get; set; }
}

public class UpdateSurfDto : BaseUpdateSurfDto
{
    public Maturity Maturity { get; set; }
}
