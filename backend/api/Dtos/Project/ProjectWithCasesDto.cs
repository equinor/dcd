using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class ProjectWithCasesDto : ProjectDto
{
    [Required]
    public ICollection<CaseDto> Cases { get; set; } = [];
    [Required]
    public ICollection<ProjectDto> Revisions { get; set; } = [];
}
