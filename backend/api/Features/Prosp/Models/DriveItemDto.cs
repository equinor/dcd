using System.ComponentModel.DataAnnotations;

namespace api.Features.Prosp.Models;

public class DriveItemDto
{
    [Required] public required string Name { get; set; }
    [Required] public required string Id { get; set; }
}
