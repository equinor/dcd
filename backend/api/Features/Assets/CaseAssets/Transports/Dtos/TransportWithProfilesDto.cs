using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Dtos;

public class TransportWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public TransportCostProfileDto CostProfile { get; set; } = new();
    [Required]
    public TransportCostProfileOverrideDto CostProfileOverride { get; set; } = new();
    [Required]
    public TransportCessationCostProfileDto CessationCostProfile { get; set; } = new();
    [Required]
    public Maturity Maturity { get; set; }
    [Required]
    public double GasExportPipelineLength { get; set; }
    [Required]
    public double OilExportPipelineLength { get; set; }
    [Required]
    public Currency Currency { get; set; }
    public DateTime? LastChangedDate { get; set; }
    [Required]
    public int CostYear { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
}

public class TransportCostProfileDto : TimeSeriesCostDto;

public class TransportCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class TransportCessationCostProfileDto : TimeSeriesCostDto;
