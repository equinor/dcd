using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class ProjectWithCasesDto : ProjectDto
{
    [Required]
    public ICollection<CaseDto> Cases { get; set; } = [];
    [Required]
    public ICollection<ProjectDto> Revisions { get; set; } = [];
}
