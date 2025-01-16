using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.WellProjects.GasInjectorCostProfileOverrides.Dtos;
using api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides.Dtos;
using api.Features.Profiles.WellProjects.OilProducerCostProfileOverrides.Dtos;
using api.Features.Profiles.WellProjects.WaterInjectorCostProfileOverrides.Dtos;
using api.Models;

namespace api.Features.Stea.Dtos;

public class WellProjectWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public OilProducerCostProfileDto OilProducerCostProfile { get; set; } = new();
    [Required]
    public OilProducerCostProfileOverrideDto OilProducerCostProfileOverride { get; set; } = new();
    [Required]
    public GasProducerCostProfileDto GasProducerCostProfile { get; set; } = new();
    [Required]
    public GasProducerCostProfileOverrideDto GasProducerCostProfileOverride { get; set; } = new();
    [Required]
    public WaterInjectorCostProfileDto WaterInjectorCostProfile { get; set; } = new();
    [Required]
    public WaterInjectorCostProfileOverrideDto WaterInjectorCostProfileOverride { get; set; } = new();
    [Required]
    public GasInjectorCostProfileDto GasInjectorCostProfile { get; set; } = new();
    [Required]
    public GasInjectorCostProfileOverrideDto GasInjectorCostProfileOverride { get; set; } = new();
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public List<WellProjectWellDto> WellProjectWells { get; set; } = [];
}

public class OilProducerCostProfileDto : TimeSeriesCostDto;

public class GasProducerCostProfileDto : TimeSeriesCostDto;

public class WaterInjectorCostProfileDto : TimeSeriesCostDto;

public class GasInjectorCostProfileDto : TimeSeriesCostDto;
