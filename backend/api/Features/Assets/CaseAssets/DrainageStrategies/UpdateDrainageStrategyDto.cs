using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.DrainageStrategies;

public class UpdateDrainageStrategyDto
{
    [Required] public double NGLYield { get; set; }
    [Required] public double GasShrinkageFactor { get; set; }
    [Required] public int ProducerCount { get; set; }
    [Required] public int GasInjectorCount { get; set; }
    [Required] public int WaterInjectorCount { get; set; }
    [Required] public ArtificialLift ArtificialLift { get; set; }
    [Required] public GasSolution GasSolution { get; set; }
}
