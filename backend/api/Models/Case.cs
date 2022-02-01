using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{

    public class Case
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public string Description { get; set; } = string.Empty!;
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset ModifyTime { get; set; }
        public Boolean ReferenceCase { get; set; }

        public DateTimeOffset DG4Date { get; set; }
        public Project Project { get; set; } = null!;
        public Guid DrainageStrategyLink { get; set; }
        public Guid WellProjectLink { get; set; }
        public Guid SurfLink { get; set; }
        public Guid SubstructureLink { get; set; }
        public Guid TopsideLink { get; set; }
        public Guid TransportLink { get; set; }
        public Guid ExplorationLink { get; set; }
    }

    public enum ArtificialLift
    {
        NoArtificialLift,
        GasLift,
        ElectricalSubmergedPumps,
        SubseaBoosterPumps
    }
    public class CessationCost : TimeSeriesCost<double>
    {
        [ForeignKey("Case.Id")]
        public Case Case { get; set; } = null!;
    }
}
