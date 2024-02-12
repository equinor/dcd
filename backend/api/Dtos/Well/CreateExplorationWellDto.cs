
using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class CreateExplorationWellDto
{
    public DrillingScheduleDto? DrillingSchedule { get; set; }
    [Required]
    public Guid ExplorationId { get; set; } = Guid.Empty!;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty!;
}
