
namespace api.Models;

public class ExplorationWell
{
    public virtual DrillingSchedule? DrillingSchedule { get; set; }
    public Guid? DrillingScheduleId { get; set; }
    public virtual Exploration Exploration { get; set; } = null!;
    public Guid ExplorationId { get; set; } = Guid.Empty;
    public virtual Well Well { get; set; } = null!;
    public Guid WellId { get; set; } = Guid.Empty;
}
