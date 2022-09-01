using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UploadController : ControllerBase
{
    // private const string testDriveItemId = "01LF7VUDUW3IAIVUAVBNAJALIVG7JK62EZ";
    private readonly GraphRestService _graphRestService;
    private readonly ImportProspService _prospService;

    public UploadController(ImportProspService prospService, GraphRestService graphRestService)
    {
        _prospService = prospService;
        _graphRestService = graphRestService;
    }

    [HttpGet(Name = nameof(GetSharePointFileNamesAndId))]
    public List<DriveItemDto> GetSharePointFileNamesAndId([FromQuery] string url)
    {
        return _graphRestService.GetFilesFromSite();
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

    [HttpPost("sharepoint", Name = nameof(ImportFromSharepoint))]
    [DisableRequestSizeLimit]
    public ProjectDto? ImportFromSharepoint([FromQuery] Guid projectId, [FromBody] SharePointImportDto[] dto)
    {
        foreach (var item in dto)
        {
            Console.WriteLine(item.Id);
        }

        try
        {
            var projectDto = new ProjectDto();
            foreach (var fileInfo in dto)
            {
                var stream = _graphRestService.GetSharepointFileStream(fileInfo.SharePointFileId);

                if (stream.Length > 0)
                {
                    projectDto = _prospService.ImportProsp(stream, new Guid(fileInfo.Id!), projectId);
                }
            }
            return projectDto;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
