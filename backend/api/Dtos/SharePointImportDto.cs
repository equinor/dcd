namespace api.Dtos;

public class SharePointImportDto
{
    public string? Id { get; set; }
    public bool Surf { get; set; }
    public bool Substructure { get; set; }
    public bool Topside { get; set; }
    public bool Transport { get; set; }
    public string SharePointFileName { get; set; } = null!;
    public string SharePointFileId { get; set; } = null!;
    public string SharePointSiteUrl { get; set; } = null!;
}
