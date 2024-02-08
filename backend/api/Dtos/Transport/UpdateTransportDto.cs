using System.ComponentModel.DataAnnotations;

using api.Models;
namespace api.Dtos;

public class UpdateTransportDto
{
    public string Name { get; set; } = string.Empty!;
    public TransportCostProfileDto CostProfile { get; set; } = new TransportCostProfileDto();
    public TransportCostProfileOverrideDto CostProfileOverride { get; set; } = new TransportCostProfileOverrideDto();
    public TransportCessationCostProfileDto CessationCostProfile { get; set; } = new TransportCessationCostProfileDto();
    public Maturity Maturity { get; set; }
    public double GasExportPipelineLength { get; set; }
    public double OilExportPipelineLength { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
}

public class UpdateTransportCostProfileDto : TimeSeriesCostDto
{

}
public class UpdateTransportCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateTransportCessationCostProfileDto : TimeSeriesCostDto
{

}
