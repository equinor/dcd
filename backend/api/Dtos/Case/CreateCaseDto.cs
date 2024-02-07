using api.Models;

namespace api.Dtos;

public class CreateCaseDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public DateTimeOffset DG4Date { get; set; }
}