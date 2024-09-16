
namespace api.Models;

public class WellProjectWell
{
    public virtual DrillingSchedule? DrillingSchedule { get; set; }
    public Guid? DrillingScheduleId { get; set; }
    public virtual WellProject WellProject { get; set; } = null!;
    public Guid WellProjectId { get; set; } = Guid.Empty!;
    public virtual Well Well { get; set; } = null!;
    public Guid WellId { get; set; } = Guid.Empty!;
}
