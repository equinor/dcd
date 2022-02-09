using System.ComponentModel.DataAnnotations.Schema;

using api.Context;
using api.Models;
namespace api.Dtos
{
    public class SurfDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Guid ProjectId { get; set; }
        public Guid SourceCaseId { get; set; }
        public SurfCostProfileDto CostProfile { get; set; } = null!;
        public Maturity Maturity { get; set; }
        public double InfieldPipelineSystemLength { get; set; }
        public double UmbilicalSystemLength { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public int RiserCount { get; set; }
        public int TemplateCount { get; set; }
        public ProductionFlowline ProductionFlowline { get; set; }
    }

    public enum ProductionFlowlineDto
    {
        Default = 999
    }

    public class SurfCostProfileDto : TimeSeriesCost<double>
    {

    }
}
