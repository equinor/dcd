
namespace api.Dtos;

public class ExplorationWellDto
{
    public DrillingScheduleDto? DrillingSchedule { get; set; }
    public Guid ExplorationId { get; set; } = Guid.Empty!;
    public Guid WellId { get; set; } = Guid.Empty!;
    public bool HasChanges { get; set; }
}
