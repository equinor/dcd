using System.ComponentModel.DataAnnotations;

using api.Dtos;

namespace api.Features.Projects.GetWithCases;

public class ProjectWithCasesDto : ProjectDto
{
    [Required] public List<CaseDto> Cases { get; set; } = [];
    [Required] public List<ProjectDto> Revisions { get; set; } = [];
}
