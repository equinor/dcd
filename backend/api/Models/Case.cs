using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{

    public class Case
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty!;
        public string Description { get; set; } = string.Empty!;
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset ModifyTime { get; set; }
        public bool ReferenceCase { get; set; }
        public DateTimeOffset DG0Date { get; set; }
        public DateTimeOffset DG1Date { get; set; }
        public DateTimeOffset DG2Date { get; set; }
        public DateTimeOffset DG3Date { get; set; }
        public DateTimeOffset DG4Date { get; set; }
        public Project Project { get; set; } = null!;
        public ArtificialLift ArtificialLift { get; set; }
        public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
        public int ProducerCount { get; set; }
        public int GasInjectorCount { get; set; }
        public int WaterInjectorCount { get; set; }
        public double FacilitiesAvailability { get; set; }
        public Guid DrainageStrategyLink { get; set; } = Guid.Empty;
        public Guid WellProjectLink { get; set; } = Guid.Empty;
        public Guid SurfLink { get; set; } = Guid.Empty;
        public Guid SubstructureLink { get; set; } = Guid.Empty;
        public Guid TopsideLink { get; set; } = Guid.Empty;
        public Guid TransportLink { get; set; } = Guid.Empty;
        public Guid ExplorationLink { get; set; } = Guid.Empty;
        public Well Well { get; set; } = null!;
    }

    public enum ArtificialLift
    {
        NoArtificialLift,
        GasLift,
        ElectricalSubmergedPumps,
        SubseaBoosterPumps
    }
    public enum ProductionStrategyOverview
    {
        Depletion,
        WaterInjection,
        GasInjection,
        WAG,
        Mixed
    }
    public class CessationCost : TimeSeriesCost
    {
        [ForeignKey("Case.Id")]
        public Case Case { get; set; } = null!;
    }
}
