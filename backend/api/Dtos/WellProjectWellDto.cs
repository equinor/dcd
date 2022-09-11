namespace api.Dtos;

public class WellProjectWellDto
{
    public int Count { get; set; }
    public DrillingScheduleDto? DrillingSchedule { get; set; }
    public Guid WellProjectId { get; set; } = Guid.Empty!;
    public Guid WellId { get; set; } = Guid.Empty!;
}
