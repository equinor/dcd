using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class WellProject
{
    public Guid Id { get; set; }
    public Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public OilProducerCostProfile? OilProducerCostProfile { get; set; }
    public GasProducerCostProfile? GasProducerCostProfile { get; set; }
    public WaterInjectorCostProfile? WaterInjectorCostProfile { get; set; }
    public GasInjectorCostProfile? GasInjectorCostProfile { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
    public ICollection<WellProjectWell>? WellProjectWells { get; set; }
}

public class OilProducerCostProfile : TimeSeriesCost
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
}

public class GasProducerCostProfile : TimeSeriesCost
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
}

public class WaterInjectorCostProfile : TimeSeriesCost
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
}

public class GasInjectorCostProfile : TimeSeriesCost
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
}
