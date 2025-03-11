using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Cases.Create;

public class CreateCaseDto
{
    [Required] public required string Name { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    [Required] public required int ProducerCount { get; set; }
    [Required] public required int GasInjectorCount { get; set; }
    [Required] public required int WaterInjectorCount { get; set; }
    [Required] public required DateTime Dg4Date { get; set; }
}
