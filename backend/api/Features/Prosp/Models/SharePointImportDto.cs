using System.ComponentModel.DataAnnotations;

namespace api.Features.Prosp.Models;

public class SharePointImportDto
{
    [Required] public required Guid CaseId { get; set; }
    [Required] public required string SharePointFileId { get; set; }
    [Required] public required string SharePointFileName { get; set; }
    [Required] public required string SharePointSiteUrl { get; set; }
}
