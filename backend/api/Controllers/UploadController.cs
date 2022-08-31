using System.Net.Http.Headers;

using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly ImportProspService _prospService;
        private readonly GraphRestService _graphRestService;
        private const string testDriveItemId = "01LF7VUDUW3IAIVUAVBNAJALIVG7JK62EZ";

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

        [HttpPost(Name = nameof(Upload)), DisableRequestSizeLimit]
        public async Task<ProjectDto?> Upload([FromQuery] Guid projectId, [FromQuery] Guid sourceCaseId)
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
}
