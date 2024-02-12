using api.Models;
namespace api.Dtos;

public class UpdateTransportDto
{
    public string Name { get; set; } = string.Empty!;
    public UpdateTransportCostProfileOverrideDto CostProfileOverride { get; set; } = new UpdateTransportCostProfileOverrideDto();
    public Maturity Maturity { get; set; }
    public double GasExportPipelineLength { get; set; }
    public double OilExportPipelineLength { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
}

public class UpdateTransportCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
