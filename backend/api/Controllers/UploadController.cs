using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UploadController : ControllerBase
{
    private const string testDriveItemId = "01LF7VUDUW3IAIVUAVBNAJALIVG7JK62EZ";
    private readonly GraphRestService _graphRestService;
    private readonly ImportProspService _prospService;

    public UploadController(ImportProspService prospService, GraphRestService graphRestService)
    {
        _prospService = prospService;
        _graphRestService = graphRestService;
    }

    [HttpGet(Name = nameof(GetFilesFromSharePoint))]
    public List<DriveItemDto> GetFilesFromSharePoint()
    {
        var graph = _graphRestService.GetFilesFromSite();
        return graph;
    }

    [HttpPost(Name = nameof(Upload))]
    [DisableRequestSizeLimit]
    public ProjectDto? Upload([FromQuery] Guid projectId, [FromQuery] Guid sourceCaseId)
    {
        var graph = _graphRestService.GetFilesFromSite();
        try
        {
            var stream = _graphRestService.GetSharepointFileStream(testDriveItemId);

            if (stream.Length > 0)
            {
                var dto = _prospService.ImportProsp(stream, sourceCaseId, projectId);
                return dto;
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
