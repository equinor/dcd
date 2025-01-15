using System.ComponentModel.DataAnnotations;

using api.Models;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;

public class OnshorePowerSupplyDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    public DateTime? LastChangedDate { get; set; }
    [Required]
    public int CostYear { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
}
