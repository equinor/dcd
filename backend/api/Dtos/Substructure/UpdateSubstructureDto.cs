using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateSubstructureDto
{
    public string Name { get; set; } = string.Empty!;
    public UpdateSubstructureCostProfileOverrideDto? CostProfileOverride { get; set; }
    public double DryWeight { get; set; }
    public Maturity Maturity { get; set; }
    public Currency Currency { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public int CostYear { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public Concept Concept { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }

}


public class UpdateSubstructureCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
