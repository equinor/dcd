using System.ComponentModel.DataAnnotations;

namespace api.Features.Images.Get;

public class ImageDto
{
    [Required] public required Guid ImageId { get; set; }
    [Required] public required DateTime CreateTime { get; set; }
    public required string? Description { get; set; }
    [Required] public required Guid? CaseId { get; set; }
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required string ImageData { get; set; }
}
