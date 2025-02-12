using api.Context;
using api.Context.Extensions;
using api.Features.Stea.Dtos;
using api.Features.Stea.ExcelExport;

namespace api.Features.Stea;

public class SteaService(DcdDbContext context, SteaRepository steaRepository)
{
    public async Task<(byte[] excelFileContents, string filename)> GetExcelFile(Guid projectId)
    {
        var project = await GetInputToStea(projectId);
        var businessCases = ExportToExcelService.CreateExcelCells(project);
        var bytes = ExportToExcelService.WriteExcelCellsToExcelDocument(businessCases, project.Name);

        return (bytes, project.Name + "ExportToSTEA.xlsx");
    }

    private async Task<SteaProjectDto> GetInputToStea(Guid projectId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var project = await steaRepository.GetProjectWithCasesAndProfiles(projectPk);

        var steaCaseDtos = project.Cases.Select(SteaCaseDtoBuilder.Build).ToList();

        var startYears = steaCaseDtos.Where(x => x.StartYear > 0).Select(x => x.StartYear).ToList();

        return new SteaProjectDto
        {
            Name = project.Name,
            SteaCases = steaCaseDtos,
            StartYear = startYears.Any() ? startYears.Min() : 0
        };
    }
}
