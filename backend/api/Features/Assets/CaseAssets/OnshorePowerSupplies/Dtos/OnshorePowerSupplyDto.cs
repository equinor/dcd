using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;

public class OnshorePowerSupplyDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    [Required]
    public int CostYear { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
}
