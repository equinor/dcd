
namespace api.Models;

public class WellProjectWell
{
    public DrillingSchedule? DrillingSchedule { get; set; }
    public Guid? DrillingScheduleId { get; set; }
    public WellProject WellProject { get; set; } = null!;
    public Guid WellProjectId { get; set; } = Guid.Empty!;
    public Well Well { get; set; } = null!;
    public Guid WellId { get; set; } = Guid.Empty!;
}
