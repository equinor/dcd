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
