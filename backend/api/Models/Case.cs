
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
        public int ProducerCount { get; set; }
        public int GasInjectorCount { get; set; }
        public int WaterInjectorCount { get; set; }
        public int RiserCount { get; set; }
        public int TemplateCount { get; set; }
        public double FacilitiesAvailability { get; set; }
        public virtual CessationCost CessationCost { get; set; } = null!;
        public DateTimeOffset DG4Date { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }

    public enum ArtificialLift
    {
        NoArtificialLift,
        GasLift,
        ElectricalSubmergedPumps,
        SubseaBoosterPumps
    }
}
