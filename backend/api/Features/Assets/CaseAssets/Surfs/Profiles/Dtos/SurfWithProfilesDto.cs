using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos;

public class SurfWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public SurfCostProfileDto CostProfile { get; set; } = new();
    [Required]
    public SurfCostProfileOverrideDto CostProfileOverride { get; set; } = new();
    [Required]
    public SurfCessationCostProfileDto CessationCostProfile { get; set; } = new();
    [Required]
    public double CessationCost { get; set; }
    [Required]
    public Maturity Maturity { get; set; }
    [Required]
    public double InfieldPipelineSystemLength { get; set; }
    [Required]
    public double UmbilicalSystemLength { get; set; }
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public int RiserCount { get; set; }
    [Required]
    public int TemplateCount { get; set; }
    [Required]
    public int ProducerCount { get; set; }
    [Required]
    public int GasInjectorCount { get; set; }
    [Required]
    public int WaterInjectorCount { get; set; }
    [Required]
    public ProductionFlowline ProductionFlowline { get; set; }
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public DateTime LastChangedDate { get; set; }
    [Required]
    public int CostYear { get; set; }
    [Required]
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
    [Required]
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
}

public class SurfCostProfileDto : TimeSeriesCostDto;

public class SurfCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class SurfCessationCostProfileDto : TimeSeriesCostDto;
