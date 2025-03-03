using api.Context;
using api.Context.Extensions;
using api.Features.Stea.Dtos;
using api.Features.Stea.ExcelExport;

namespace api.Features.Stea;

public class SteaService(DcdDbContext context, SteaRepository steaRepository)
{
    public async Task<(byte[] excelFileContents, string filename)> GetExcelFile(Guid projectId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);
        var project = await steaRepository.GetProjectWithCasesAndProfiles(projectPk);

        var steaCaseDtos = project.Cases.Select(SteaCaseDtoBuilder.Build).ToList();

        var bytes = ExportToExcelService.ExportToExcel(project.Name, steaCaseDtos);

        return (bytes, project.Name + "ExportToSTEA.xlsx");
    }
}
