using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ImportProspService _prospService;
        public UploadController(ImportProspService prospService)
        {
            _prospService = prospService;
        }

        [HttpPost(Name = "Upload"), DisableRequestSizeLimit]
        public async Task<ProjectDto> Upload([FromQuery] Guid projectId, [FromQuery] Guid sourceCaseId)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                if (file.Length > 0)
                {
                    var dto = _prospService.ImportProsp(file, sourceCaseId, projectId);
                    return dto;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}