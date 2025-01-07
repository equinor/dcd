using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Dtos.Update;

public abstract class BaseUpdateSubstructureDto
{
    public double DryWeight { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public Concept Concept { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
}

