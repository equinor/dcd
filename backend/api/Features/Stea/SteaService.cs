using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Stea.Dtos;
using api.Features.Stea.ExcelExport;
using api.Features.Wells.Get;
using api.Models;

using AutoMapper;

namespace api.Features.Stea;

public class SteaService(DcdDbContext context, SteaRepository steaRepository, IMapper mapper)
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

        var project = await steaRepository.GetProjectWithCasesAndAssets(projectPk);

        var drainageStrategies = await steaRepository.GetDrainageStrategies(projectPk);
        var explorations = await steaRepository.GetExplorations(projectPk);
        var onshorePowerSupplies = await steaRepository.GetOnshorePowerSupplies(projectPk);
        var substructures = await steaRepository.GetSubstructures(projectPk);
        var surfs = await steaRepository.GetSurfs(projectPk);
        var topsides = await steaRepository.GetTopsides(projectPk);
        var transports = await steaRepository.GetTransports(projectPk);
        var wellProjects = await steaRepository.GetWellProjects(projectPk);
        var wells = await steaRepository.GetWells(projectPk);

        var data = new ProjectWithAssetsWrapperDto
        {
            Project = mapper.Map<Project, ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString()),
            DrainageStrategies = mapper.Map<List<DrainageStrategyWithProfilesDto>>(drainageStrategies, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString()),
            Explorations = mapper.Map<List<ExplorationWithProfilesDto>>(explorations),
            OnshorePowerSupplies = mapper.Map<List<OnshorePowerSupplyWithProfilesDto>>(onshorePowerSupplies),
            Substructures = mapper.Map<List<SubstructureWithProfilesDto>>(substructures),
            Surfs = mapper.Map<List<SurfWithProfilesDto>>(surfs),
            Topsides = mapper.Map<List<TopsideWithProfilesDto>>(topsides),
            Transports = mapper.Map<List<TransportWithProfilesDto>>(transports),
            WellProjects = mapper.Map<List<WellProjectWithProfilesDto>>(wellProjects),
            Wells = mapper.Map<List<WellDto>>(wells)
        };

        var steaCaseDtos = data.Project
            .Cases
            .Where(x => !x.Archived)
            .Select(x => SteaCaseDtoBuilder.Build(x, data))
            .ToList();

        return SteaProjectDtoBuilder.Build(project.Name, steaCaseDtos);
    }
}
