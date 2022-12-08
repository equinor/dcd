using api.Models;

namespace api.Dtos;

public class SubstructureDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty!;
    public Guid ProjectId { get; set; }
    public SubstructureCostProfileDto? CostProfile { get; set; }
    public SubstructureCessationCostProfileDto? CessationCostProfile { get; set; }
    public double DryWeight { get; set; }
    public Maturity Maturity { get; set; }
    public Currency Currency { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public int CostYear { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public Source Source { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    public Concept Concept { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
    public bool HasChanges { get; set; }
}

public class SubstructureCostProfileDto : TimeSeriesCostDto
{
}

public class SubstructureCessationCostProfileDto : TimeSeriesCostDto
{
}
