using System.ComponentModel.DataAnnotations;

using api.Features.ProjectAccess;

namespace api.Features.ProjectData.Dtos;

public class RevisionDataDto
{
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required Guid RevisionId { get; set; }
    [Required] public required string DataType { get; set; }
    [Required] public required UserActionsDto UserActions { get; set; }
    [Required] public required RevisionDetailsDto RevisionDetails { get; set; }
    [Required] public required List<RevisionDetailsDto> RevisionDetailsList { get; set; }
    [Required] public required CommonProjectAndRevisionDto CommonProjectAndRevisionData { get; set; }
}
