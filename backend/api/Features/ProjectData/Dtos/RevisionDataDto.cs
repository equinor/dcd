using System.ComponentModel.DataAnnotations;

using api.Features.Revisions.Get;

namespace api.Features.ProjectData.Dtos;

public class RevisionDataDto
{
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required Guid RevisionId { get; set; }
    [Required] public required RevisionDetailsDto RevisionDetails { get; set; }

    [Required] public required CommonProjectAndRevisionDto CommonProjectAndRevisionData { get; set; }
}
