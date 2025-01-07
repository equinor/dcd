using System.ComponentModel.DataAnnotations;

namespace api.Features.ProjectData.Dtos;

public class ActionsDto
{
    [Required] public required bool CanView { get; set; }
    [Required] public required bool CanCreateRevision { get; set; }
}
