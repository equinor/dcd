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

        var data = new SteaDbData
        {
            Project = await steaRepository.GetProjectWithCasesAndAssets(projectPk),
            DrainageStrategies = await steaRepository.GetDrainageStrategies(projectPk),
            Explorations = await steaRepository.GetExplorations(projectPk),
            OnshorePowerSupplies = await steaRepository.GetOnshorePowerSupplies(projectPk),
            Substructures = await steaRepository.GetSubstructures(projectPk),
            Surfs = await steaRepository.GetSurfs(projectPk),
            Topsides = await steaRepository.GetTopsides(projectPk),
            Transports = await steaRepository.GetTransports(projectPk),
            WellProjects = await steaRepository.GetWellProjects(projectPk)
        };

        var steaCaseDtos = new List<SteaCaseDto>();

        foreach (var caseItem in data.Project.Cases)
        {
            if (caseItem.Archived)
            {
                continue;
            }

            steaCaseDtos.Add(SteaCaseDtoBuilder.Build(caseItem, data));
        }

        var startYears = steaCaseDtos.Where(x => x.StartYear > 0).Select(x => x.StartYear).ToList();

        return new SteaProjectDto
        {
            Name = data.Project.Name,
            SteaCases = steaCaseDtos,
            StartYear = startYears.Any() ? startYears.Min() : 0
        };
    }
}
