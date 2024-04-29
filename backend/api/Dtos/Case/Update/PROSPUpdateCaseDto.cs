namespace api.Dtos;

public class PROSPUpdateCaseDto : BaseUpdateCaseDto
{
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
}
