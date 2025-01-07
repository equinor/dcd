using System.ComponentModel.DataAnnotations;

using api.Features.ProjectMembers.Get;

namespace api.Features.ProjectData.Dtos;

public class ProjectDataDto
{
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required string DataType { get; set; }
    [Required] public required bool HasAccess { get; set; }
    [Required] public required ActionsDto Actions { get; set; }
    [Required] public required List<ProjectMemberDto> ProjectMembers { get; set; }
    [Required] public required List<RevisionDetailsDto> RevisionDetailsList { get; set; }

    [Required] public required CommonProjectAndRevisionDto CommonProjectAndRevisionData { get; set; }
}
