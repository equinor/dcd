using System.ComponentModel.DataAnnotations;

namespace api.Features.ProjectData.Dtos;

public class RevisionDataDto
{
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required Guid RevisionId { get; set; }
    [Required] public required string DataType { get; set; }
    [Required] public required bool HasAccess { get; set; }
    [Required] public required ActionsDto Actions { get; set; }
    [Required] public required RevisionDetailsDto RevisionDetails { get; set; }

    [Required] public required CommonProjectAndRevisionDto CommonProjectAndRevisionData { get; set; }
}
