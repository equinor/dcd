
using System.ComponentModel.DataAnnotations;

namespace api.Features.CaseProfiles.Dtos.Well;

public class UpdateExplorationWellDto
{
    public UpdateDrillingScheduleDto? DrillingSchedule { get; set; }
    [Required]
    public Guid ExplorationId { get; set; } = Guid.Empty;
    [Required]
    public Guid WellId { get; set; } = Guid.Empty;
}
