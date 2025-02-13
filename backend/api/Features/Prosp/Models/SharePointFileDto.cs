using System.ComponentModel.DataAnnotations;

namespace api.Features.Prosp.Models;

public class SharePointFileDto
{
    [Required] public required string Name { get; set; }
    [Required] public required string Id { get; set; }
}
