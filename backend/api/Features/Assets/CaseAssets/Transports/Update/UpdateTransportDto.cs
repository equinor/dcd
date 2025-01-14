using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Update;

public class UpdateTransportDto
{
    public double GasExportPipelineLength { get; set; }
    public double OilExportPipelineLength { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Source Source { get; set; }
    public Maturity Maturity { get; set; }
}

public class ProspUpdateTransportDto
{
    public double GasExportPipelineLength { get; set; }
    public double OilExportPipelineLength { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
}
