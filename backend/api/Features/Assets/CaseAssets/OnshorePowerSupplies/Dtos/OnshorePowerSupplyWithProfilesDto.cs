using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;

public class OnshorePowerSupplyWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public OnshorePowerSupplyCostProfileDto CostProfile { get; set; } = new();
    [Required]
    public OnshorePowerSupplyCostProfileOverrideDto CostProfileOverride { get; set; } = new();
    [Required]
    public OnshorePowerSupplyCessationCostProfileDto CessationCostProfile { get; set; } = new();
    public DateTimeOffset? LastChangedDate { get; set; }
    [Required]
    public int CostYear { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
}

public class OnshorePowerSupplyCostProfileDto : TimeSeriesCostDto;

public class OnshorePowerSupplyCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class OnshorePowerSupplyCessationCostProfileDto : TimeSeriesCostDto;
