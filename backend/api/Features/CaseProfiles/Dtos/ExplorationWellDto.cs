
using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.Well;

namespace api.Features.CaseProfiles.Dtos;

public class ExplorationWellDto
{
    [Required]
    public DrillingScheduleDto DrillingSchedule { get; set; } = new();
    [Required]
    public Guid ExplorationId { get; set; } = Guid.Empty;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty;
}
