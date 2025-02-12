using api.Models;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureDto
{
    public double DryWeight { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public Concept Concept { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}
