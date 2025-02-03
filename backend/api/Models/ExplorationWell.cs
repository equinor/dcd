
using api.Models.Interfaces;

namespace api.Models;

public class ExplorationWell : IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid WellId { get; set; }
    public virtual Well Well { get; set; } = null!;

    public Guid? DrillingScheduleId { get; set; }
    public virtual DrillingSchedule? DrillingSchedule { get; set; }

    public Guid ExplorationId { get; set; }
    public virtual Exploration Exploration { get; set; } = null!;

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
}
