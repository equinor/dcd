
using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class UpdateWellProjectWellWithScheduleDto
{
    public DrillingScheduleDto DrillingSchedule { get; set; } = new();
    [Required]
    public Guid WellProjectId { get; set; } = Guid.Empty;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty;
}
