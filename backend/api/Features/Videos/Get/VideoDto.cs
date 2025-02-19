using System.ComponentModel.DataAnnotations;

namespace api.Features.Videos.Get;

public class VideoDto
{
    [Required] public required string VideoName { get; set; }
    [Required] public required string Base64EncodedData { get; set; }
}
