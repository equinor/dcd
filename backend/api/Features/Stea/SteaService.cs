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

    public async Task<SteaProjectDto> GetInputToStea(Guid projectId)
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
            WellProjects = await steaRepository.GetWellProjects(projectPk),
            Wells = await steaRepository.GetWells(projectPk)
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

        return SteaProjectDtoBuilder.Build(data.Project.Name, steaCaseDtos);
    }
}
