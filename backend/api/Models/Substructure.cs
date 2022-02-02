using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Substructure
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        [ForeignKey("Project.Id")]
        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public SubstructureCostProfile CostProfile { get; set; } = null!;
        public double DryWeight { get; set; }
        public Maturity Maturity { get; set; }
    }

    public class SubstructureCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Substructure.Id")]
        public Substructure Substructure { get; set; } = null!;
    }
}
