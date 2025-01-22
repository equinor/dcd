using api.Models.Interfaces;

namespace api.Models;

public class DrillingSchedule : TimeSeriesSchedule, IDateTrackedEntity
{
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
}
