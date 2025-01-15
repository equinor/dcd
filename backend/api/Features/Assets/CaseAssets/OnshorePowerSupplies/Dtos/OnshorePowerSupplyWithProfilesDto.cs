using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;

public class OnshorePowerSupplyWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public OnshorePowerSupplyCostProfileDto CostProfile { get; set; } = new();
    [Required]
    public OnshorePowerSupplyCostProfileOverrideDto CostProfileOverride { get; set; } = new();
    public DateTime? LastChangedDate { get; set; }
    [Required]
    public int CostYear { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
}

public class OnshorePowerSupplyCostProfileDto : TimeSeriesCostDto;

public class OnshorePowerSupplyCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
