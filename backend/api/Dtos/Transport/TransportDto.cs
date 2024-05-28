using System.ComponentModel.DataAnnotations;

using api.Models;
namespace api.Dtos;

public class TransportDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty!;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public Maturity Maturity { get; set; }
    [Required]
    public double GasExportPipelineLength { get; set; }
    [Required]
    public double OilExportPipelineLength { get; set; }
    [Required]
    public Currency Currency { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    [Required]
    public int CostYear { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
}
