
using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class ExplorationWellDto
{
    [Required]
    public DrillingScheduleDto? DrillingSchedule { get; set; }
    [Required]
    public Guid ExplorationId { get; set; } = Guid.Empty!;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty!;
    public bool HasChanges { get; set; }
}
