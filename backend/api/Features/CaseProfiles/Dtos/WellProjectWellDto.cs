
using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.Well;

namespace api.Features.CaseProfiles.Dtos;

public class WellProjectWellDto
{
    [Required]
    public DrillingScheduleDto DrillingSchedule { get; set; } = new();
    [Required]
    public Guid WellProjectId { get; set; } = Guid.Empty;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty;
}
