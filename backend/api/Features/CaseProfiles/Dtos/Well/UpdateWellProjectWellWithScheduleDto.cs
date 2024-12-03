
using System.ComponentModel.DataAnnotations;

namespace api.Features.CaseProfiles.Dtos.Well;

public class UpdateWellProjectWellWithScheduleDto
{
    public DrillingScheduleDto DrillingSchedule { get; set; } = new();
    [Required]
    public Guid WellProjectId { get; set; } = Guid.Empty;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty;
}
