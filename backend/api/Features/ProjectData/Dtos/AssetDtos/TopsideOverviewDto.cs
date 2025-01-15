using System.ComponentModel.DataAnnotations;

using api.Models;
using api.Models.Enums;

namespace api.Features.ProjectData.Dtos.AssetDtos;

public class TopsideOverviewDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required double FuelConsumption { get; set; }
    [Required] public required double CO2ShareOilProfile { get; set; }
    [Required] public required double CO2ShareGasProfile { get; set; }
    [Required] public required double CO2ShareWaterInjectionProfile { get; set; }
    [Required] public required double CO2OnMaxOilProfile { get; set; }
    [Required] public required double CO2OnMaxGasProfile { get; set; }
    [Required] public required double CO2OnMaxWaterInjectionProfile { get; set; }
    [Required] public required Source Source { get; set; }
}
