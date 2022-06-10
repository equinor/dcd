using api.Models;
namespace api.Dtos
{
    public class TransportDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Guid ProjectId { get; set; }
        public TransportCostProfileDto? CostProfile { get; set; }
        public TransportCessationCostProfileDto? CessationCostProfile { get; set; }
        public Maturity Maturity { get; set; }
        public double GasExportPipelineLength { get; set; }
        public double OilExportPipelineLength { get; set; }
        public Currency Currency { get; set; }
        public DateTimeOffset? LastChangedDate { get; set; }
        public int CostYear { get; set; }
        public Source Source { get; set; }
        public DateTimeOffset? ProspVersion { get; set; }
        public DateTimeOffset? DG3Date { get; set; }
        public DateTimeOffset? DG4Date { get; set; }
    }

    public class TransportCostProfileDto : TimeSeriesCostDto
    {

    }

    public class TransportCessationCostProfileDto : TimeSeriesCostDto
    {

    }
}
