using System.Net.Http.Headers;

using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly ImportProspService _prospService;
        private readonly GraphRestService _graphRestService;

        public UploadController(ImportProspService prospService, GraphRestService graphRestService)
        {
            _prospService = prospService;
            _graphRestService = graphRestService;
        }

        [HttpGet(Name = "GetFilesFromSharePoint")]
        public List<Stream> GetFilesFromSharePoint()
        {
            var graph = _graphRestService.GetAllFilesFromSite();
            return graph;
        }

        [HttpPost(Name = "Upload"), DisableRequestSizeLimit]
        public async Task<ProjectDto?> Upload([FromQuery] Guid projectId, [FromQuery] Guid sourceCaseId)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var assets = new Dictionary<string, bool>()
                {
                    {"Surf", false},
                    {"Topside", false},
                    {"Substructure", false},
                    {"Transport", false},

                };

                if (file.Length > 0)
                {
                    if (formCollection.TryGetValue("Surf", out var surf) && surf == "true")
                    {
                        assets["Surf"] = true;
                    }
                    if (formCollection.TryGetValue("Topside", out var topside) && topside == "true")
                    {
                        assets["Topside"] = true;
                    }
                    if (formCollection.TryGetValue("Substructure", out var substructure) && substructure == "true")
                    {
                        assets["Substructure"] = true;
                    }
                    if (formCollection.TryGetValue("Transport", out var transport) && transport == "true")
                    {
                        assets["Transport"] = true;
                    }
                    var dto = _prospService.ImportProsp(file, sourceCaseId, projectId, assets);
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
