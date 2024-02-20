using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class SubstructureDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty!;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public SubstructureCostProfileDto CostProfile { get; set; } = new SubstructureCostProfileDto();
    [Required]
    public SubstructureCostProfileOverrideDto CostProfileOverride { get; set; } = new SubstructureCostProfileOverrideDto();
    [Required]
    public SubstructureCessationCostProfileDto CessationCostProfile { get; set; } = new SubstructureCessationCostProfileDto();
    [Required]
    public double DryWeight { get; set; }
    [Required]
    public Maturity Maturity { get; set; }
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public string ApprovedBy { get; set; } = string.Empty;
    [Required]
    public int CostYear { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    [Required]
    public Concept Concept { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
    public bool HasChanges { get; set; }
}

public class SubstructureCostProfileDto : TimeSeriesCostDto
{
}
public class SubstructureCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class SubstructureCessationCostProfileDto : TimeSeriesCostDto
{
}
