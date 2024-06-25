
using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class WellProjectWellDto
{
    [Required]
    public DrillingScheduleDto DrillingSchedule { get; set; } = new DrillingScheduleDto();
    [Required]
    public Guid WellProjectId { get; set; } = Guid.Empty!;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty!;
}
