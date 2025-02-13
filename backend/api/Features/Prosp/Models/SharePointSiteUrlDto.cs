using System.ComponentModel.DataAnnotations;

namespace api.Features.Prosp.Models;

public class SharePointSiteUrlDto
{
    [Required] public required string Url { get; set; }
}
