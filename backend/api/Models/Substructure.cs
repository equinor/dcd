using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Substructure
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public SubstructureCostProfile? CostProfile { get; set; }
        public SubstructureCessationCostProfile? CessationCostProfile { get; set; }
        public double DryWeight { get; set; }
        public Maturity Maturity { get; set; }
        public Currency Currency { get; set; }
        public string ApprovedBy { get; set; } = string.Empty;
        public int CostYear { get; set; }
        public DateTimeOffset? ProspVersion { get; set; }
        public Source Source { get; set; }
        public DateTimeOffset? LastChangedDate { get; set; }
        public Concept Concept { get; set;}
    }

    public enum Concept
    {
        NO_CONCEPT,
        TIE_BACK,
        JACKET,
        GBS,
        TLP,
        SPAR,
        SEMI,
        CIRCULAR_BARGE,
        BARGE,
        FPSO,
        TANKER,
        JACK_UP,
        SUBSEA_TO_SHORE
    }

    public class SubstructureCostProfile : TimeSeriesCost
    {
        [ForeignKey("Substructure.Id")]
        public Substructure Substructure { get; set; } = null!;
    }

    public class SubstructureCessationCostProfile : TimeSeriesCost
    {
        [ForeignKey("Substructure.Id")]
        public Substructure Substructure { get; set; } = null!;
    }
}
