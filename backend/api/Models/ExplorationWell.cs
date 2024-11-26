
namespace api.Models;

public class ExplorationWell
{

    public Guid WellId { get; set; }
    public virtual Well Well { get; set; } = null!;

    public Guid? DrillingScheduleId { get; set; }
    public virtual DrillingSchedule? DrillingSchedule { get; set; }

    public Guid ExplorationId { get; set; }
    public virtual Exploration Exploration { get; set; } = null!;
}
