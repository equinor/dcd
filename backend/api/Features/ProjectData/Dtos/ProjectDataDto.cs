using System.ComponentModel.DataAnnotations;

using api.Features.ProjectMembers.Get;

namespace api.Features.ProjectData.Dtos;

public class ProjectDataDto
{
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required List<ProjectMemberDto> ProjectMembers { get; set; }
    [Required] public required List<RevisionDetailsDto> RevisionDetailsList { get; set; }

    [Required] public required CommonProjectAndRevisionDto CommonProjectAndRevisionData { get; set; }
}
