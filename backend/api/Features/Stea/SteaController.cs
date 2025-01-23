using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Stea;

public class SteaController(SteaService steaService) : ControllerBase
{
    [HttpPost("stea/{projectId:guid}")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<FileResult> ExportSteaToExcel(Guid projectId)
    {
        var (bytes, filename) = await steaService.GetExcelFile(projectId);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
    }
}
