using System.ComponentModel.DataAnnotations;

using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.GetWithAssets;

public class SubstructureDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Guid ProjectId { get; set; }
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
    public DateTime? ProspVersion { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTime? LastChangedDate { get; set; }
    [Required]
    public Concept Concept { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
}
