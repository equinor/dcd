
using api.Models.Interfaces;

namespace api.Models;

public class WellProjectWell : IDateTrackedEntity
{
    public Guid? DrillingScheduleId { get; set; }
    public virtual DrillingSchedule? DrillingSchedule { get; set; }

    public Guid WellProjectId { get; set; }
    public virtual WellProject WellProject { get; set; } = null!;

    public Guid WellId { get; set; }
    public virtual Well Well { get; set; } = null!;

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
}
