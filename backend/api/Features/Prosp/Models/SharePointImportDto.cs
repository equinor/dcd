namespace api.Features.Prosp.Models;

public class SharePointImportDto
{
    public string? Id { get; set; }
    public string SharePointFileId { get; set; } = null!;
    public string? SharePointFileName { get; set; }
    public string? SharePointFileUrl { get; set; }
    public string? SharePointSiteUrl { get; set; }
}
