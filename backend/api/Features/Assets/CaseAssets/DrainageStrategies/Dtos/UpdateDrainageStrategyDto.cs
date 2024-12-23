using api.Models;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;

public class UpdateDrainageStrategyDto
{
    public double NGLYield { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public GasSolution GasSolution { get; set; }
}
