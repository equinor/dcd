using System.ComponentModel.DataAnnotations;

namespace api.Features.Prosp.Models;

public class UrlDto
{
    [Required] public required string Url { get; set; }
}
