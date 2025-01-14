using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Update;

public class UpdateSubstructureDto
{
    public double DryWeight { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public Concept Concept { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}

public class ProspUpdateSubstructureDto
{
    public required double DryWeight { get; set; }
    public required Currency Currency { get; set; }
    public required int CostYear { get; set; }
    public required Source Source { get; set; }
    public required Concept Concept { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }
    public required DateTime? ProspVersion { get; set; }
}
